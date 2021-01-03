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

        public Startup(IConfiguration configuration)
        {
            _logger = Program._loggerFactory.CreateLogger<Startup>();
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var ip = Configuration.GetValue<string>("ipaddress") ?? "10.0.0.201";
            _logger.LogInformation($"Connecting to ATEM at IP {ip} (use the 'ipaddress' command line argument to set this value)");

            services.AddSignalR();
            services.AddMediatR(typeof(Startup));
            services.AddControllersWithViews();
            services.AddTransient<AtemHubContext>();
            services.AddTransient<MessageNotificationHandler>();
            services.AddSingleton(services => 
            {
                return new SwitcherBuilder(services.GetRequiredService<IMediator>())
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
                    if (System.IO.Directory.Exists(System.IO.Path.Join(GetRootPath(), folder)))
                    {
                        configuration.RootPath = System.IO.Path.Join(GetRootPath(), folder);
                        continue;
                    }
                }
            });

            services.AddHostedService<AtemWorker>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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
        }

        string GetRootPath()
        {
            var pathToExe = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
            var pathToContentRoot = System.IO.Path.GetDirectoryName(pathToExe);
            return pathToContentRoot;
        }
    }
}
