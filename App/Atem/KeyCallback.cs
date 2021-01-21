using BMDSwitcherAPI;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwitcherServer.Atem
{
    public class KeyCallback : IBMDSwitcherKeyCallback
    {
        private readonly IMediator _mediator;

        public KeyCallback(IMediator mediator)
        {
            _mediator = mediator;
        }

        public void Notify(_BMDSwitcherKeyEventType eventType)
        {
            switch (eventType)
            {
                case _BMDSwitcherKeyEventType.bmdSwitcherKeyEventTypeOnAirChanged:
                    _mediator.Publish(new NextTransitionNotify { });
                    break;
                default:
                    _mediator.Publish(new SwitcherMessageNotify { Message = $"Key says: {eventType}" });
                    break;
            }
        }
    }
}
