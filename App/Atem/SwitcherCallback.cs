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
                    _mediator.Publish(new SwitcherMessageNotify { Message = $"Switcher Disconnected! ({ coreVideoMode })" });
                    break;
                default:
                    _mediator.Publish(new ConnectionChangeNotify { Connected = false });
                    _mediator.Publish(new SwitcherMessageNotify { Message = eventType.ToString() });
                    break;
            }
        }
    }
}
