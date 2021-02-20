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
        private readonly IMediator _mediator;

        public SwitcherBuilder(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

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
            var mediator = _mediator ?? _serviceProvider.GetRequiredService<IMediator>();
            var logger = _serviceProvider.GetService<ILogger<SwitcherConnectionKeeper>>();
            var connection = new SwitcherConnectionKeeper(mediator, logger);
            var result = new Switcher(connection, mediator);
            connection.Connect(_ipaddress);
            return result;
        }
    }
}
