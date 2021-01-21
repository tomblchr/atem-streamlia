using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwitcherServer.Atem
{
    public class NextTransitionKey
    {
        public int Key { get; set; }

        public bool OnAir { get; set; }

        public bool Included { get; set; }
    }
}
