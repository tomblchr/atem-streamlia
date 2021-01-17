using BMDSwitcherAPI;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwitcherServer.Atem
{
    public class DownstreamKeyCallback : IBMDSwitcherDownstreamKeyCallback
    {
        private readonly IBMDSwitcherDownstreamKey _downstreamKey;
        private IMediator _mediator;

        public DownstreamKeyCallback(IBMDSwitcherDownstreamKey downstreamKey, IMediator mediator)
        {
            _downstreamKey = downstreamKey;
            _mediator = mediator;
        }

        public void Notify(_BMDSwitcherDownstreamKeyEventType eventType)
        {
            switch (eventType)
            {
                case _BMDSwitcherDownstreamKeyEventType.bmdSwitcherDownstreamKeyEventTypeOnAirChanged:
                case _BMDSwitcherDownstreamKeyEventType.bmdSwitcherDownstreamKeyEventTypeTieChanged:
                    _mediator.Publish(new InputChangeNotify());
                    break;
                case _BMDSwitcherDownstreamKeyEventType.bmdSwitcherDownstreamKeyEventTypeIsAutoTransitioningChanged:
                    _downstreamKey.IsAutoTransitioning(out int IsAutoTransitioning);
                    _mediator.Publish(new DownstreamKeyAutoTransitionNotify { IsTransitioning = IsAutoTransitioning == 1 });
                    break;
                default:
                    _mediator.Publish(new SwitcherMessageNotify { Message = eventType.ToString() });
                    break;
            }
        }
    }
}
