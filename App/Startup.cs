using BMDSwitcherAPI;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SwitcherServer.Atem;
using System;
using System.ComponentModel;

namespace SwitcherServer
{
    public class Startup
    {
        readonly ILogger _logger;

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            _logger = Program._loggerFactory.CreateLogger<Startup>();
            Configuration = configuration;
            Environment = environment;
        }

        public IConfiguration Configuration { get; }

        public IWebHostEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var ip = Configuration.GetValue<string>("AtemIpAddress");
            _logger.LogInformation($"Connecting to ATEM at IP {ip} (use the '--AtemIpAddress=...' command line argument or appsettings.json to set this value)");

            services.AddSignalR();
            services.AddControllersWithViews();
            services.AddMediatR(typeof(Startup));
            services.AddTransient<AtemHubContext>();
            services.AddSingleton<SwitcherConnectionKeeper>();

            services.AddSingleton<IConnectionChangeNotifyQueue, ConnectionChangeNotificationQueue>();
            services.AddSingleton<IVolumeChangeNotificationQueue, VolumeChangeNotificationQueue>();
            services.AddHostedService<AtemWorker>();            

            if (Environment.IsDevelopment())
            {
                services.AddTransient<MessageNotificationHandler>();
            }

            services.AddSingleton(services => 
            {
                return new SwitcherBuilder(services)
                    .NetworkIP(ip)
                    .Build();
            });

            services.AddCors(options => options
                .AddPolicy("CorsPolicy", builder => builder
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()));

            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/public";

                // check other locations
                var folders = new[]
                {
                    "ClientApp/public",
                    "ClientApp/build"
                };
                foreach (var folder in folders)
                {
                    if (System.IO.Directory.Exists(System.IO.Path.Join(Program.GetRootPath(), folder)))
                    {
                        configuration.RootPath = System.IO.Path.Join(Program.GetRootPath(), folder);
                        continue;
                    }
                }
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime life)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseCors("CorsPolicy");
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");

                endpoints.MapHub<AtemHub>("/atemhub");
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });

            life.ApplicationStarted.Register(OnStarted);
            life.ApplicationStopping.Register(OnShutdown);
        }

        public void OnStarted()
        {
            _logger.LogInformation($"----------- Open a broswer at {NetworkInspector.GetUrl(Configuration)} ---------------");
        }

        public void OnShutdown()
        {
            _logger.LogInformation("Disconnecting...");

            _logger.LogInformation("Shutdown sequence complete.");
        }

    }
}
