using BMDSwitcherAPI;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwitcherServer.Atem
{
    public class MacroPoolCallback : IBMDSwitcherMacroPoolCallback
    {
        private readonly IMediator _mediator;

        public MacroPoolCallback(IMediator mediator)
        {
            _mediator = mediator;
        }

        public void Notify(_BMDSwitcherMacroPoolEventType eventType, uint index, IBMDSwitcherTransferMacro macroTransfer)
        {
            switch (eventType)
            {
                default:
                    _mediator.Publish(new SwitcherMessageNotify { Message = $"Macro Pool: {eventType}=>{index}" });
                    break;
            }
        }
    }
}
