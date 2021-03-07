using BMDSwitcherAPI;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwitcherServer.Atem
{
    public class KeyFlyParametersNotify : INotification
    {
        public KeyFlyParametersNotify(bool isRunning, _BMDSwitcherFlyKeyFrame destination)
        {
            IsRunning = isRunning;
            Destination = destination;
        }

        public bool IsRunning { get; }

        public _BMDSwitcherFlyKeyFrame Destination { get; }
    }
}
