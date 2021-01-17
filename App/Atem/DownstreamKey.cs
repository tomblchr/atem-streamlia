using BMDSwitcherAPI;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwitcherServer.Atem
{
    public class DownstreamKey
    {
        private readonly IBMDSwitcherDownstreamKey _key;
        private readonly IMediator _mediator;

        public DownstreamKey(IBMDSwitcherDownstreamKey key, IMediator mediator)
        {
            _key = key;
            _mediator = mediator;

            _key.AddCallback(new DownstreamKeyCallback(_key, _mediator));
        }

        public bool OnAir
        {
            get
            {
                _key.GetOnAir(out int onAir);
                return onAir == 1;
            }
            set
            {
                _key.SetOnAir(value ? 1 : 0);
            }
        }

        public bool Tie
        {
            get
            {
                _key.GetTie(out int tie);
                return tie == 1;
            }
            set
            {
                _key.SetTie(value ? 1 : 0);
            }
        }

        public void DoSomething()
        {
            _key.PerformAutoTransition();
        }
    }
}
