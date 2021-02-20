using BMDSwitcherAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwitcherServer.Atem
{
    interface ISwitcherConnection
    {
        IBMDSwitcher Connect();

        IBMDSwitcher Connect(string ipaddress);

        bool IsConnected { get; }

        void Disconnect();
    }
}
