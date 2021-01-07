using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwitcherServer.Atem
{
    public class MasterOutLevelNotify : INotification
    {
        public uint NumLevels { get; set; }

        public double Levels { get; set; }
    }
}
