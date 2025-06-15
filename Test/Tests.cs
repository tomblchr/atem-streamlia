using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using SwitcherServer.Atem;
using System;
using System.Collections.Generic;
using System.Text;

namespace SwitcherServerTests
{
    public class Tests
    {
        static readonly ILogger _logger;
        static readonly Switcher _switcher;

        static Tests()
        {
            using var factory = LoggerFactory
                .Create(builder => builder
                    .AddConsole()
                    .SetMinimumLevel(LogLevel.Information));

            _logger = factory.CreateLogger<Tests>();

            var ip = SwitcherServer.Program
                .BuildConfiguration<Tests>([])
                .GetValue<string>("AtemIpAddress");

            _switcher = new SwitcherBuilder()
                .Mediator(new MockMediator())
                .ConnectionKeeper(new SwitcherConnectionKeeper(new MockMediator(), null))
                .NetworkIP(ip)
                .Build();
        }

        protected ILogger Logger => _logger;

        protected Switcher AtemMini => _switcher;
    }
}
