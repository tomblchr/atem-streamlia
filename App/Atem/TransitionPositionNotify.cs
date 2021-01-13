using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwitcherServer.Atem
{
    public class TransitionPositionNotify : INotification
    {
        public double Position { get; set; }

        public long FramesRemaining { get; set; }
    }
}
