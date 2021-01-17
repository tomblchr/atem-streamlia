using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwitcherServer.Atem
{
    public class DownstreamKeyAutoTransitionNotify : INotification
    {
        public bool IsTransitioning { get; set; }
    }
}
