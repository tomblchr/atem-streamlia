using BMDSwitcherAPI;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwitcherServer.Atem
{
    public class KeyFlyParametersCallback : IBMDSwitcherKeyFlyParametersCallback
    {
        private readonly IMediator _mediator;

        public KeyFlyParametersCallback(IMediator mediator)
        {
            _mediator = mediator;
        }

        public void Notify(_BMDSwitcherKeyFlyParametersEventType eventType, _BMDSwitcherFlyKeyFrame keyFrame)
        {
            _mediator.Publish(new SwitcherMessageNotify { Message = $"KeyFlyParameters says: {eventType}:{keyFrame} " });
        }
    }
}
