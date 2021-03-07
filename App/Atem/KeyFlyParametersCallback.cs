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
        private readonly KeyFlyParameters _keyFlyParameters;
        private readonly IMediator _mediator;

        public KeyFlyParametersCallback(KeyFlyParameters keyFlyParameters, IMediator mediator)
        {
            _keyFlyParameters = keyFlyParameters;
            _mediator = mediator;
        }

        public void Notify(_BMDSwitcherKeyFlyParametersEventType eventType, _BMDSwitcherFlyKeyFrame keyFrame)
        {
            switch (eventType)
            {
                case _BMDSwitcherKeyFlyParametersEventType.bmdSwitcherKeyFlyParametersEventTypeIsRunningChanged:
                    _keyFlyParameters.Switcher.IsRunning(out int isRunning, out _BMDSwitcherFlyKeyFrame destination);
                    _mediator.Publish(new KeyFlyParametersNotify(isRunning == 1, destination));
                    break;
                default:
                    _mediator.Publish(new SwitcherMessageNotify { Message = $"KeyFlyParameters says: {eventType}:{keyFrame} " });
                    break;
            }
        }
    }
}
