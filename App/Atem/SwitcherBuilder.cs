using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwitcherServer.Atem
{
    public class SwitcherBuilder
    {
        private string _ipaddress = "10.0.0.201";
        private readonly IMediator _mediator;

        public SwitcherBuilder(IMediator mediator)
        {
            _mediator = mediator;
        }

        public SwitcherBuilder NetworkIP(string ipaddress)
        {
            _ipaddress = ipaddress;
            return this;
        }

        public Switcher Build()
        {
            var connection = new SwitcherConnectionKeeper(_ipaddress, _mediator);
            var result = new Switcher(connection, _mediator);
            return result;
        }
    }
}
