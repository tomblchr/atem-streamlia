using BMDSwitcherAPI;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwitcherServer.Atem
{
    public class SwitcherCallback : IBMDSwitcherCallback
    {
        private readonly IMediator _mediator;

        public SwitcherCallback(IMediator mediator)
        {
            _mediator = mediator;
        }

        public void Notify(_BMDSwitcherEventType eventType, _BMDSwitcherVideoMode coreVideoMode)
        {
            switch (eventType)
            {
                case _BMDSwitcherEventType.bmdSwitcherEventTypeDisconnected:
                    _mediator.Publish(new ConnectionChangeNotify { Connected = false });
                    break;
                case _BMDSwitcherEventType.bmdSwitcherEventTypeTimeCodeChanged:
                    break;
                default:
                    _mediator.Publish(new SwitcherMessageNotify { Message = $"Switcher says: {eventType}" });
                    break;
            }
        }
    }
}
