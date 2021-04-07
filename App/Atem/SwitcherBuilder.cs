using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwitcherServer.Atem
{
    public class SwitcherBuilder
    {
        private string _ipaddress = "10.0.0.201";
        private readonly IServiceProvider _serviceProvider;
        private IMediator _mediator;
        private SwitcherConnectionKeeper _connection;

        public SwitcherBuilder()
        {

        }

        public SwitcherBuilder(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public SwitcherBuilder Mediator(IMediator mediator)
        {
            _mediator = mediator;
            return this;
        }

        public SwitcherBuilder ConnectionKeeper(SwitcherConnectionKeeper switcherConnectionKeeper)
        {
            _connection = switcherConnectionKeeper;
            return this;
        }

        public SwitcherBuilder NetworkIP(string ipaddress)
        {
            _ipaddress = ipaddress;
            return this;
        }

        public Switcher Build()
        {
            var mediator = _mediator ?? _serviceProvider?.GetRequiredService<IMediator>();
            var connection = _connection ?? _serviceProvider?.GetRequiredService<SwitcherConnectionKeeper>();

            var result = new Switcher(connection, mediator);

            //result.Reset(_ipaddress);

            return result;
        }
    }
}
