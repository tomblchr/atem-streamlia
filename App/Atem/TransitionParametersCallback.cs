using BMDSwitcherAPI;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwitcherServer.Atem
{
    public class TransitionParametersCallback : IBMDSwitcherTransitionParametersCallback
    {
        private readonly IBMDSwitcherTransitionParameters _transitionParameters;
        private readonly IMediator _mediator;

        public TransitionParametersCallback(IBMDSwitcherTransitionParameters transitionParameters, IMediator mediator)
        {
            _transitionParameters = transitionParameters;
            _mediator = mediator;
        }

        public void Notify(_BMDSwitcherTransitionParametersEventType eventType)
        {
            switch (eventType)
            {
                case _BMDSwitcherTransitionParametersEventType.bmdSwitcherTransitionParametersEventTypeTransitionStyleChanged:
                    _transitionParameters.GetTransitionStyle(out _BMDSwitcherTransitionStyle style);
                    _mediator.Publish(new TransitionStyleNotify { Current = style });
                    break;
                default:
                    _mediator.Publish(new SwitcherMessageNotify { Message = eventType.ToString() });
                    break;
            }
        }
    }
}
