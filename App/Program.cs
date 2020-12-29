using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwitcherServer
{
    public class Program
    {
        public static ILoggerFactory _loggerFactory;
        static ILogger _logger;

        public static void Main(string[] args)
        {
            try
            {
                using var factory = LoggerFactory
                    .Create(builder => builder
                        .AddConsole()
                        .SetMinimumLevel(LogLevel.Information));

                _loggerFactory = factory;
                _logger = factory.CreateLogger<Program>();

                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception e)
            {
                _logger.LogError("Application Error: ", e);
            }
            finally
            {
                _logger.LogInformation("Exiting. Have a good day.");
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.AddConsole();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
