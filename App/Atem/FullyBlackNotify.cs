using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwitcherServer.Atem
{
    public class FullyBlackNotify : INotification
    {
        public bool IsFullyBlack { get; set; }

        public bool IsInTransition { get; set; }
    }
}
