using BMDSwitcherAPI;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwitcherServer.Atem
{
    public class StreamRTMPCallback : IBMDSwitcherStreamRTMPCallback
    {
        private readonly IMediator _mediator;

        public StreamRTMPCallback(IMediator mediator)
        {
            _mediator = mediator;
        }

        public void Notify(_BMDSwitcherStreamRTMPEventType eventType)
        {
            _mediator.Publish(new SwitcherMessageNotify { Message = $"{nameof(StreamRTMPCallback)}-{eventType}" });
        }

        public void NotifyStatus(_BMDSwitcherStreamRTMPState stateType, _BMDSwitcherStreamRTMPError error)
        {
            _mediator.Publish(new StreamRTMPStatusNotify { Status = stateType });
        }
    }
}
