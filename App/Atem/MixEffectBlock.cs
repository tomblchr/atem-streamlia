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
                Switcher.GetProgramInput(out long value);
                return _switcher.GetInputs().Single(c => c.Id == value);
            }
        }

        public Input PreviewInput
        {
            get
            {
                Switcher.GetPreviewInput(out long value);
                return _switcher.GetInputs().Single(c => c.Id == value);
            }
        }

        private IEnumerable<Key> _keys;
        public IEnumerable<Key> Keys
        {
            get
            {
                if (_keys == null)
                {
                    var keys = Switcher.GetKeys();
                    _keys = keys.Select(c => new Key(c, _mediator)).ToList();
                }
                return _keys;
            }
        }

        public NextTransition GetNextTransition()
        {
            return new NextTransition(Keys, _transitionParameters);
        }
    }
}
