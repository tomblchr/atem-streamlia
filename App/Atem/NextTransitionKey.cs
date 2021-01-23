using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwitcherServer.Atem
{
    public class NextTransitionKey
    {
        public NextTransitionKey(int key, bool onAir, bool included)
        {
            Key = key;
            OnAir = onAir;
            Included = included;
        }

        public int Key { get; private set; }

        public bool OnAir { get; private set; }

        public bool Included { get; private set; }
    }
}
