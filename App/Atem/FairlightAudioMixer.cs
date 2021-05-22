using BMDSwitcherAPI;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwitcherServer.Atem
{
    public class FairlightAudioMixer : IDisposable
    {
        private readonly IBMDSwitcherFairlightAudioMixer _mixer;
        private readonly IMediator _mediator;
        private readonly IBMDSwitcherFairlightAudioMixerCallback _callback;
        private bool _hasCallback = false;

        public FairlightAudioMixer(IBMDSwitcherFairlightAudioMixer mixer, IMediator mediator)
        {
            _mixer = mixer;
            _mediator = mediator;

            Init();

            _callback = new FairlightAudioMixerCallback(_mediator);
            _mixer.AddCallback(_callback);
            _hasCallback = true;
        }

        void Init()
        {
            _mixer.SetAllLevelNotificationsEnabled(1);

            if (_fairlightAudioMixerInputs == null)
            {
                var inputs = _mixer.GetFairlightAudioMixerInputs();
                _fairlightAudioMixerInputs = inputs.Select(c => new FairlightAudioMixerInput(c, _mediator)).ToList();
            }
        }

        public void Dispose()
        {
            if (_hasCallback)
            {
                _mixer.RemoveCallback(_callback);
                _hasCallback = false;
            }
            _fairlightAudioMixerInputs?.ToList().ForEach(c => c.Dispose());
            _fairlightAudioMixerInputs = null;
        }

        private IEnumerable<FairlightAudioMixerInput> _fairlightAudioMixerInputs;
        public IEnumerable<FairlightAudioMixerInput> Inputs
        {
            get
            {
                return _fairlightAudioMixerInputs;
            }
        }

        public bool MasterOutFollowFadeToBlack
        {
            get 
            {
                _mixer.GetMasterOutFollowFadeToBlack(out int follow);
                return follow == 1;
            }
            set 
            {
                _mixer.SetMasterOutFollowFadeToBlack(value ? 1 : 0);
            }
        }

        public double MasterOutFaderGain
        {
            get
            {
                _mixer.GetMasterOutFaderGain(out double gain);
                return gain;
            }
            set
            {
                _mixer.SetMasterOutFaderGain(value);
            }
        }
    }
}
