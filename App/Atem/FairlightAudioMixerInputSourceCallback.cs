using BMDSwitcherAPI;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwitcherServer.Atem
{
    public class FairlightAudioMixerInputSourceCallback : IBMDSwitcherFairlightAudioSourceCallback
    {
        private readonly FairlightAudioMixerInputSource _source;
        private readonly IMediator _mediator;

        public FairlightAudioMixerInputSourceCallback(FairlightAudioMixerInputSource source, IMediator mediator)
        {
            _source = source;
            _mediator = mediator;
        }

        public void Notify(_BMDSwitcherFairlightAudioSourceEventType eventType)
        {
            _mediator.Publish(new SwitcherMessageNotify { Message = $"{nameof(FairlightAudioMixerInputSourceCallback)}-{eventType}" });
        }

        public void OutputLevelNotification(uint numLevels, ref double levels, uint numPeakLevels, ref double peakLevels)
        {
            var l = BlackMagicDesignSdk.ConvertDoubleArray(numLevels, ref levels);
            var p = BlackMagicDesignSdk.ConvertDoubleArray(numPeakLevels, ref peakLevels);

            _mediator.Publish(new VolumeLevelNotify(_source.Input.Id, _source.Id, numLevels, l, numPeakLevels, p));
        }
    }
}
