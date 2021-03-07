using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
                _logger.LogError(e, "Application Error");
            }
            finally
            {
                _logger.LogInformation("Done!");
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
                    webBuilder.UseKestrel(options =>
                    {
                         options.ListenAnyIP(5001, configure =>
                        {
                            configure.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http1;
#if DEBUG
                            // will use the dotnetcore development certificates
                            configure.UseHttps();
#else
                            var csi = new CertificateStoreInspector(_loggerFactory.CreateLogger<CertificateStoreInspector>());
                            var certificate = csi.GetCertificate("Root", StoreLocation.CurrentUser) 
                                ?? csi.GetCertificate("My", StoreLocation.CurrentUser);

                            if (certificate != null)
                            {
                                configure.UseHttps(certificate);
                            }
#endif
                        });
                    });
                });
    }
}
