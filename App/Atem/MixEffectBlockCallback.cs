﻿using BMDSwitcherAPI;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwitcherServer.Atem
{
    public class MixEffectBlockCallback : IBMDSwitcherMixEffectBlockCallback
    {
        private readonly IMediator _mediator;

        public MixEffectBlockCallback(IMediator mediator)
        {
            _mediator = mediator;
        }

        public void Notify(_BMDSwitcherMixEffectBlockEventType eventType)
        {
            switch (eventType)
            {
                case _BMDSwitcherMixEffectBlockEventType.bmdSwitcherMixEffectBlockEventTypeProgramInputChanged:
                    _mediator.Publish(new InputChangeNotify());
                    break;
            }
        }
    }
}
