using BMDSwitcherAPI;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwitcherServer.Atem
{
    public class StreamRTMPStatusNotify : INotification
    {
        public _BMDSwitcherStreamRTMPState Status { get; set; }
    }
}
