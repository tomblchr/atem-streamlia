using BMDSwitcherAPI;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwitcherServer.Atem
{
    public class RecordAV
    {
        private readonly IBMDSwitcherRecordAV _recorder;
        private readonly IMediator _mediator;

        public RecordAV(IBMDSwitcherRecordAV stream, IMediator mediator)
        {
            _recorder = stream;
            _mediator = mediator;

            _recorder?.AddCallback(new RecordAVCallback(mediator));
        }

        public bool IsRecording 
        {
            get
            {
                _recorder.IsRecording(out int recording);
                return recording == 1;
            } 
        }

        public _BMDSwitcherRecordAVState Status
        {
            get
            {
                _recorder.GetStatus(out _BMDSwitcherRecordAVState state, out _BMDSwitcherRecordAVError error);

                return state;
            }
        }
    }
}
