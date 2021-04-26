using BMDSwitcherAPI;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwitcherServer.Atem
{
    public class FairlightAudioMixerInputSource : IHasId
    {
        private readonly FairlightAudioMixerInput _input;
        private readonly IBMDSwitcherFairlightAudioSource _source;
        private readonly IMediator _mediator;

        public FairlightAudioMixerInputSource(FairlightAudioMixerInput input, IBMDSwitcherFairlightAudioSource source, IMediator mediator)
        {
            _input = input;
            _source = source;
            _mediator = mediator;
            source.AddCallback(new FairlightAudioMixerInputSourceCallback(this, _mediator));
        }

        public long Id
        {
            get
            {
                _source.GetId(out long id);
                return id;
            }
        }

        public FairlightAudioMixerInput Input
        {
            get
            {
                return _input;
            }
        }

        public bool IsActive
        {
            get
            {
                _source.IsActive(out int active);
                return active == 1;
            }
        }

        public double FaderGain
        {
            get
            {
                _source.GetFaderGain(out double gain);
                return gain;
            }
            set
            {
                _source.SetFaderGain(value);
            }
        }

        public double InputGain
        {
            get
            {
                _source.GetInputGain(out double gain);
                return gain;
            }
            set
            {
                _source.SetInputGain(value);
            }
        }

        public _BMDSwitcherFairlightAudioMixOption MixOption
        {
            get
            {
                _source.GetMixOption(out _BMDSwitcherFairlightAudioMixOption option);
                return option;
            }
            set
            {
                _source.SetMixOption(value);
            }
        }
    }
}
