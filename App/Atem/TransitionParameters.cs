using BMDSwitcherAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwitcherServer.Atem
{
    public class TransitionParameters
    {
        private readonly IBMDSwitcherTransitionParameters _transitionParameters;

        public TransitionParameters(IBMDSwitcherTransitionParameters transitionParameters)
        {
            _transitionParameters = transitionParameters;
        }

        public bool BackgroundIncluded
        {
            get
            {
                _transitionParameters.GetNextTransitionSelection(out var selection);
                return selection.HasFlag(_BMDSwitcherTransitionSelection.bmdSwitcherTransitionSelectionBackground);
            }
            set
            {
                _transitionParameters.GetNextTransitionSelection(out var selection);
                selection = selection | _BMDSwitcherTransitionSelection.bmdSwitcherTransitionSelectionBackground;
                _transitionParameters.SetNextTransitionSelection(selection);
            }
        }
    }
}
