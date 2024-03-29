﻿using BMDSwitcherAPI;
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

        public long Id 
        { 
            get
            {
                _bmd.GetInputId(out long id);
                return id;
            }
        }

        public string Name 
        { 
            get
            {
                _bmd.GetShortName(out string name);
                return name;
            }
        }

        public string LongName
        {
            get
            {
                _bmd.GetLongName(out string name);
                return name;
            }
        }

        public _BMDSwitcherPortType InputType
        {
            get
            {
                _bmd.GetPortType(out _BMDSwitcherPortType portType);
                return portType;
            }
        }
    }
}
