using BMDSwitcherAPI;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwitcherServer.Atem
{
    public class FairlightAudioMixerCallback : IBMDSwitcherFairlightAudioMixerCallback
    {
        private readonly IMediator _mediator;

        public FairlightAudioMixerCallback(IMediator mediator)
        {
            _mediator = mediator;
        }

        public void Notify(_BMDSwitcherFairlightAudioMixerEventType eventType)
        {
            _mediator.Publish(new SwitcherMessageNotify { Message = $"{nameof(FairlightAudioMixerCallback)}-{eventType}" });
        }

        public void MasterOutLevelNotification(uint numLevels, ref double levels, uint numPeakLevels, ref double peakLevels)
        {
            _mediator.Publish(new MasterOutLevelNotify { NumLevels = numLevels, Levels = levels });
        }
    }
}
