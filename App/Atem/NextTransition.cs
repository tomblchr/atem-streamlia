using BMDSwitcherAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwitcherServer.Atem
{
    public class NextTransition
    {
        public NextTransition(IEnumerable<Key> keys, IBMDSwitcherTransitionParameters _transitionParameters)
        {
            _transitionParameters.GetNextTransitionSelection(out _BMDSwitcherTransitionSelection included);

            IncludeBackground = included.HasFlag(_BMDSwitcherTransitionSelection.bmdSwitcherTransitionSelectionBackground);

            Keys = keys
                .Select(key => new NextTransitionKey(
                    (int)key.TransitionSelectionMask, 
                    key.OnAir, 
                    included.HasFlag(key.TransitionSelectionMask)))
                .OrderBy(c => c.Key);
        }

        public bool IncludeBackground { get; private set; }

        public IOrderedEnumerable<NextTransitionKey> Keys { get; private set; }
    }
}
