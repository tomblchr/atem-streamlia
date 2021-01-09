using BMDSwitcherAPI;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwitcherServer.Atem
{
    public class MixEffectBlock
    {
        private readonly IMediator _mediator;
        private readonly Switcher _switcher;
        private readonly IBMDSwitcherMixEffectBlock _bmd;
        private readonly IBMDSwitcherTransitionParameters _transitionParameters;

        public MixEffectBlock(Switcher switcher, IBMDSwitcherMixEffectBlock bmd, IMediator mediator)
        {
            _switcher = switcher;
            _bmd = bmd;
            _mediator = mediator;
            _transitionParameters = _bmd.GetTransitionParameters();

            _bmd.AddCallback(new MixEffectBlockCallback(this, _mediator));
            _transitionParameters.AddCallback(new TransitionParametersCallback(_transitionParameters, _mediator));
        }

        public IBMDSwitcherMixEffectBlock Switcher => _bmd;

        public Input ProgramInput 
        {
            get 
            {
                _bmd.GetProgramInput(out long value);
                return _switcher.GetInputs().Single(c => c.Id == value);
            }
        }

        public Input PreviewInput
        {
            get
            {
                _bmd.GetPreviewInput(out long value);
                return _switcher.GetInputs().Single(c => c.Id == value);
            }
        }
    }
}
