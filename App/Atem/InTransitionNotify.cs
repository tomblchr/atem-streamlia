using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwitcherServer.Atem
{
    public class InTransitionNotify : INotification
    {
        public bool InTransition { get; set; }
    }
}
