using BMDSwitcherAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwitcherServer.Atem
{
    public class KeyFlyParameters
    {
        private readonly IBMDSwitcherKeyFlyParameters _parameters;

        public KeyFlyParameters(IBMDSwitcherKeyFlyParameters parameters)
        {
            _parameters = parameters;
        }

        public IBMDSwitcherKeyFlyParameters Switcher => _parameters;
    }
}
