using BMDSwitcherAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwitcherServer.Atem
{
    public class Input
    {
        private readonly IBMDSwitcherInput _bmd;

        public Input(IBMDSwitcherInput bmd)
        {
            _bmd = bmd;
        }

        public IBMDSwitcherInput Switcher => _bmd;

        public long Id { get; set; }

        public string Name { get; set; }
    }
}
