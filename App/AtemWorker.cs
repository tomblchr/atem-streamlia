using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using SwitcherServer.Atem;

namespace SwitcherServer
{
    /// <summary>
    /// Keep a connection to the ATEM.
    /// </summary>
    /// <remarks>
    /// This service is connected to the connection change notification queue. When a disconnection
    /// event is plucked from the queue the reconnection attempts begin. These will proceed until
    /// the connection to the ATEM is reestablished or the application is shut down.
    /// </remarks>
    public class AtemWorker : BackgroundService
    {
        private readonly Switcher _switcher;
        private readonly IConnectionChangeNotifyQueue _queue;
        private readonly ILogger _logger;

        public AtemWorker(Switcher switcher, IConnectionChangeNotifyQueue queue, ILogger<AtemWorker> logger)
        {
            _switcher = switcher;
            _queue = queue;
            _logger = (ILogger)logger ?? NullLogger.Instance;
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var notification = await _queue.DequeueAsync(stoppingToken);
                    if (notification == null)
                    {
                        continue;
                    }
                    while (!notification.Connected && !_switcher.IsConnected && !stoppingToken.IsCancellationRequested)
                    {

                        if (!ConnectToSwitcher())
                        {
                            _logger.LogWarning("Connection could not be established. Try again shortly.");
                            Thread.Sleep(3000);
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    // this is to be expected when the system is shutting down
                }
                catch (Exception e)
                {
                    // this indicates a connection could not be made and we should try again
                    _logger.LogError(e, "Connecting to the switcher failed miserably.");
                    Thread.Sleep(3000);
                }
            }
            _logger.LogInformation("Shutting down - no need to maintain the connection anymore");
        }

        bool ConnectToSwitcher()
        {
            _logger.LogInformation("Establishing connection...");
            _switcher.Reset();
            return _switcher.IsConnected;
        }
    }
}
