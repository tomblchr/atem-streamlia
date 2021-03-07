using BMDSwitcherAPI;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwitcherServer.Atem
{
    public class KeyFlyParameters
    {
        private readonly IBMDSwitcherKeyFlyParameters _parameters;

        public KeyFlyParameters(IBMDSwitcherKeyFlyParameters parameters, IMediator mediator)
        {
            _parameters = parameters;
            _parameters.AddCallback(new KeyFlyParametersCallback(this, mediator));
        }

        public IBMDSwitcherKeyFlyParameters Switcher => _parameters;
    }
}
