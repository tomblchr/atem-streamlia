using BMDSwitcherAPI;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwitcherServer.Atem
{
    public class MixEffectBlockCallback : IBMDSwitcherMixEffectBlockCallback
    {
        private readonly MixEffectBlock _mixEffectBlock;
        private readonly IMediator _mediator;

        public MixEffectBlockCallback(MixEffectBlock mixEffectBlock, IMediator mediator)
        {
            _mixEffectBlock = mixEffectBlock;
            _mediator = mediator;
        }

        public void Notify(_BMDSwitcherMixEffectBlockEventType eventType)
        {
            switch (eventType)
            {
                case _BMDSwitcherMixEffectBlockEventType.bmdSwitcherMixEffectBlockEventTypeProgramInputChanged:
                case _BMDSwitcherMixEffectBlockEventType.bmdSwitcherMixEffectBlockEventTypePreviewInputChanged:
                    _mediator.Publish(new InputChangeNotify());
                    break;
                case _BMDSwitcherMixEffectBlockEventType.bmdSwitcherMixEffectBlockEventTypeInTransitionChanged:
                    _mixEffectBlock.Switcher.GetInTransition(out int inTransition);
                    _mediator.Publish(new InTransitionNotify { InTransition = inTransition == 1 });
                    break;
                case _BMDSwitcherMixEffectBlockEventType.bmdSwitcherMixEffectBlockEventTypeFadeToBlackFullyBlackChanged:
                case _BMDSwitcherMixEffectBlockEventType.bmdSwitcherMixEffectBlockEventTypeInFadeToBlackChanged:
                    _mixEffectBlock.Switcher.GetFadeToBlackFullyBlack(out int fullyBlack);
                    _mixEffectBlock.Switcher.GetFadeToBlackInTransition(out int inFullyBlackTransition);
                    _mediator.Publish(new FullyBlackNotify { IsFullyBlack = fullyBlack == 1, IsInTransition = inFullyBlackTransition == 1 });
                    break;
                case _BMDSwitcherMixEffectBlockEventType.bmdSwitcherMixEffectBlockEventTypeTransitionPositionChanged:
                    _mixEffectBlock.Switcher.GetTransitionPosition(out double transitionPosition);
                    _mixEffectBlock.Switcher.GetTransitionFramesRemaining(out uint transitionFramesRemaining);
                    _mediator.Publish(new TransitionPositionNotify { Position = transitionPosition, FramesRemaining = transitionFramesRemaining });
                    break;
                default:
                    _mediator.Publish(new SwitcherMessageNotify { Message = eventType.ToString() } );
                    break;
            }
        }
    }
}
