using Microsoft.Extensions.Logging;
using SwitcherServer.Atem;
using System;
using System.Collections.Generic;
using System.Text;

namespace SwitcherServerTests
{
    public class Tests
    {
        readonly ILogger _logger;
        readonly Switcher _switcher;

        public Tests()
        {
            using var factory = LoggerFactory
                .Create(builder => builder
                    .AddConsole()
                    .SetMinimumLevel(LogLevel.Information));

            _logger = factory.CreateLogger<Tests>();

            _switcher = new SwitcherBuilder()
                .Mediator(new MockMediator())
                .ConnectionKeeper(new SwitcherConnectionKeeper(new MockMediator(), null))
                .NetworkIP("192.168.68.9")
                .Build();
        }

        protected ILogger Logger => _logger;

        protected Switcher AtemMini => _switcher;
    }
}
