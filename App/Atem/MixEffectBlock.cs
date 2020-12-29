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
        private readonly IBMDSwitcherMixEffectBlock _bmd;

        public MixEffectBlock(IBMDSwitcherMixEffectBlock bmd, IMediator mediator)
        {
            _bmd = bmd;
            _mediator = mediator;
            
            _bmd.AddCallback(new MixEffectBlockCallback(_mediator));
        }

        public IBMDSwitcherMixEffectBlock Switcher => _bmd;

        public Input ProgramInput { get; set; }

        public Input PreviewInput { get; set; }
    }
}
