using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using SwitcherServer.Atem;

namespace SwitcherServer
{
    public class AtemWorker : BackgroundService
    {
        private readonly Switcher switcher;
        private readonly AtemHubContext hub;

        public AtemWorker(Switcher switcher, AtemHubContext hub)
        {
            this.switcher = switcher;
            this.hub = hub;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await DoNoWork(stoppingToken);
            }
        }

        async Task DoNoWork(CancellationToken stoppingToken)
        {
            await Task.Delay(-1, stoppingToken);
        }
    }
}
