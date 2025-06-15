using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Cryptography.X509Certificates;

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
                    var config   = BuildConfiguration<Program>(args);
                    var port     = config.GetValue<int>("HostPort", 5001);
                    var certFile = config.GetValue<string>("CertificateFile", string.Empty);
                    var password = config.GetValue<string>("CertificatePassword", string.Empty);

                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseConfiguration(config);
                    webBuilder.UseKestrel(options =>
                    {
                        options.ListenAnyIP(port, configure =>
                        {
                            configure.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http1;
#if DEBUG
                            
                            if (port == 443)
                            {
                                if (certFile == string.Empty)
                                {
                                    _logger.LogInformation($"Use the dotnetcore development certificates");
                                    configure.UseHttps();
                                }
                                else if (System.IO.File.Exists(certFile))
                                {
                                    _logger.LogInformation($"Using certificate from {certFile}");
                                    configure.UseHttps(certFile, password);
                                }
                                else
                                {
                                    _logger.LogError($"Could not find certificate file {certFile}");
                                }
                            }
#else
                            if (port == 443)
                            {
                                if (certFile == string.Empty)
                                {
                                    var csi = new CertificateStoreInspector(_loggerFactory.CreateLogger<CertificateStoreInspector>());
                                    var certificate = csi.GetCertificate("Root", StoreLocation.CurrentUser) ?? csi.GetCertificate("My", StoreLocation.CurrentUser);
                                    if (certificate == null)
                                    {
                                        _logger.LogError("Unable to find a certificate for HTTPS");
                                    }
                                    else
                                    {
                                        configure.UseHttps(certificate);
                                    }    
                                }
                                else if (System.IO.File.Exists(certFile))
                                {
                                    _logger.LogInformation($"Using certificate from {certFile}");
                                    configure.UseHttps(certFile, password);
                                }
                                else
                                {
                                    _logger.LogError($"Could not find certificate file {certFile}");
                                    configure.UseHttps();
                                }
                            }
#endif
                        });
                    });
                });

        public static IConfiguration BuildConfiguration<T>(string[] args) where T : class
        {
            return new ConfigurationBuilder()
                .SetBasePath(GetRootPath())
                .AddJsonFile("appsettings.json", true)
                .AddUserSecrets<T>()
                .AddEnvironmentVariables()
                .AddCommandLine(args)
                .Build();
        }

        public static string GetRootPath()
        {
            var pathToExe = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
            var pathToContentRoot = System.IO.Path.GetDirectoryName(pathToExe);
            return pathToContentRoot;
        }
    }
}
