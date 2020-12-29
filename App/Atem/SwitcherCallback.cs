using BMDSwitcherAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwitcherServer.Atem
{
    public class SwitcherCallback : IBMDSwitcherCallback
    {
        public SwitcherCallback()
        {
        }

        public void Notify(_BMDSwitcherEventType eventType, _BMDSwitcherVideoMode coreVideoMode)
        {
            switch (eventType)
            {
                case _BMDSwitcherEventType.bmdSwitcherEventTypePowerStatusChanged:
                case _BMDSwitcherEventType.bmdSwitcherEventTypeDisconnected:
                    break;
            }
        }
    }
}
