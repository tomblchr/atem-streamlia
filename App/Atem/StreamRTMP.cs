using BMDSwitcherAPI;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwitcherServer.Atem
{
    /// <summary>
    /// Interact with streaming capabilities of the switcher
    /// </summary>
    /// <remarks>
    /// Not all models support streaming
    /// </remarks>
    public class StreamRTMP
    {
        private readonly IBMDSwitcherStreamRTMP _stream;
        private readonly IMediator _mediator;

        public StreamRTMP(IBMDSwitcherStreamRTMP stream, IMediator mediator)
        {
            _stream = stream;
            _mediator = mediator;

            _stream?.AddCallback(new StreamRTMPCallback(mediator));
        }

        public bool IsStreamingSupported
        {
            get { return _stream != null;  }
        }

        public bool IsStreaming
        {
            get
            {
                if (IsStreamingSupported)
                {
                    _stream.IsStreaming(out int streaming);
                    return streaming == 1;
                }
                return false;
            }
        }

        public void Start()
        {
            _stream?.StartStreaming();
        }

        public void Stop()
        {
            _stream?.StopStreaming();
        }
    }
}
