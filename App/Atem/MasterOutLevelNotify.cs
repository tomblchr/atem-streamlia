using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwitcherServer.Atem
{
    public class MasterOutLevelNotify : INotification
    {
        public double[] Levels { get; set; }

        public double[] Peaks { get; set; }
    }
}
