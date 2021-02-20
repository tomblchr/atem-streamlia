using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SwitcherServer.Atem;

namespace SwitcherServer
{
    /// <summary>
    /// Keep a connection to the ATEM.
    /// </summary>
    /// <remarks>
    /// Once an initial connection to an ATEM Switcher has been established this
    /// worker will attempt to keep the connection connected.
    /// </remarks>
    public class AtemWorker : BackgroundService, INotificationHandler<ConnectionChangeNotify>
    {
        private readonly Switcher _switcher;
        private readonly IMediator _mediator;
        private readonly ILogger _logger;

        private bool _maintainConnection = false;

        public AtemWorker(Switcher switcher, IMediator mediator, ILogger<AtemWorker> logger)
        {
            _switcher = switcher;
            _mediator = mediator;
            _logger = logger;
        }

        /// <summary>
        /// Start the worker if there is no connection
        /// </summary>
        /// <param name="notification"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task Handle(ConnectionChangeNotify notification, CancellationToken cancellationToken)
        {
            if (notification.Connected)
            {
                _maintainConnection = true;
                return Task.CompletedTask;
            }

            if (!_maintainConnection)
            {
                return Task.CompletedTask;
            }
            
            _logger.LogWarning("Switcher is not connected");

            return StartAsync(cancellationToken);
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            if (_switcher.IsConnected)
            {
                return Task.CompletedTask;
            }

            return base.StartAsync(cancellationToken);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _logger.LogInformation("Establishing connection...");

                    _switcher.Reset();

                    Task.WaitAll(
                        _mediator.Publish(new ConnectionChangeNotify { Connected = _switcher.IsConnected })
                    );

                    return Task.CompletedTask;
                }
                catch (System.Runtime.InteropServices.COMException)
                {
                    // this indicates a connection could not be made and we should try again
                    _logger.LogWarning("Connection could not be established. Will wait a while and try again shortly.");
                    Thread.Sleep(1000);
                }
            }

            _logger.LogInformation("Shutting down - no need to maintain the connection anymore");
            _maintainConnection = false;

            return Task.CompletedTask;
        }
    }
}
