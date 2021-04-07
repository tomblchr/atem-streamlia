using BMDSwitcherAPI;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwitcherServer.Atem
{
    public class FairlightAudioMixer
    {
        private readonly IBMDSwitcherFairlightAudioMixer _mixer;

        public FairlightAudioMixer(IBMDSwitcherFairlightAudioMixer mixer, IMediator mediator)
        {
            _mixer = mixer;
            _mixer.SetAllLevelNotificationsEnabled(1);
            _mixer.AddCallback(new FairlightAudioMixerCallback(mediator));
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
