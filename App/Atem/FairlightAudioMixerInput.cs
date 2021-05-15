using BMDSwitcherAPI;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwitcherServer.Atem
{
    public class FairlightAudioMixerInput : IHasId, IDisposable
    {
        private readonly IBMDSwitcherFairlightAudioInput _input;
        private readonly IMediator _mediator;

        public FairlightAudioMixerInput(IBMDSwitcherFairlightAudioInput input, IMediator mediator)
        {
            _input = input;
            _mediator = mediator;

            Init();
        }

        private void Init()
        {
            if (_sources == null)
            {
                var v = _input.GetFairlightAudioMixerInputSources();
                _sources = v.Select(c => new FairlightAudioMixerInputSource(this, c, _mediator)).ToList();
            }
        }

        public void Dispose()
        {
            _sources?.ToList().ForEach(c => c.Dispose());
            _sources = null;
        }

        public long Id
        {
            get
            {
                _input.GetId(out long id);
                return id;
            }
        }

        private IEnumerable<FairlightAudioMixerInputSource> _sources;
        public IEnumerable<FairlightAudioMixerInputSource> Sources
        {
            get
            {
                return _sources;
            }
        }
    }
}
