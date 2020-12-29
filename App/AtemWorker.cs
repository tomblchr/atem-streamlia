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
        private readonly Switcher _switcher;
        private readonly AtemHubContext _hub;
        private readonly MessageNotificationHandler _message;

        public AtemWorker(Switcher switcher, AtemHubContext hub, MessageNotificationHandler message)
        {
            _switcher = switcher;
            _hub = hub;
            _message = message;
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
