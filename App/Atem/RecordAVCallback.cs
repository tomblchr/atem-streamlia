using BMDSwitcherAPI;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwitcherServer.Atem
{
    public class RecordAVCallback : IBMDSwitcherRecordAVCallback
    {
        private readonly IMediator _mediator;

        public RecordAVCallback(IMediator mediator)
        {
            _mediator = mediator;
        }

        public void Notify(_BMDSwitcherRecordAVEventType eventType)
        {
            throw new NotImplementedException();
        }

        public void NotifyWorkingSetChange(uint workingSetIndex, uint diskId)
        {
            throw new NotImplementedException();
        }

        public void NotifyDiskAvailability(_BMDSwitcherRecordDiskAvailabilityEventType eventType, uint diskId)
        {
            throw new NotImplementedException();
        }

        public void NotifyStatus(_BMDSwitcherRecordAVState stateType, _BMDSwitcherRecordAVError error)
        {
            throw new NotImplementedException();
        }
    }
}
