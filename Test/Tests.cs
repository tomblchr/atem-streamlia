using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace SwitcherServerTests
{
    public class Tests
    {
        readonly ILogger _logger;

        public Tests()
        {
            using var factory = LoggerFactory
                .Create(builder => builder
                    .AddConsole()
                    .SetMinimumLevel(LogLevel.Information));

            _logger = factory.CreateLogger<Tests>();
        }

        protected ILogger Logger => _logger;
    }
}
