using BMDSwitcherAPI;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwitcherServer.Atem
{
    public class Key
    {
        private IBMDSwitcherKey _key;

        public Key(IBMDSwitcherKey key, IMediator mediator)
        {
            _key = key;
            _key.AddCallback(new KeyCallback(mediator));
        }

        public bool OnAir
        {
            get
            {
                _key.GetOnAir(out int onAir);
                return onAir == 1;
            }
            set
            {
                _key.SetOnAir(value ? 1 : 0);
            }
        }

        /// <summary>
        /// This property uniquely identifies the key and is used when
        /// setting whether the key is included in the next transition
        /// </summary>
        public _BMDSwitcherTransitionSelection TransitionSelectionMask
        {
            get
            {
                _key.GetTransitionSelectionMask(out var mask);
                return mask;
            }
        }
    }
}
