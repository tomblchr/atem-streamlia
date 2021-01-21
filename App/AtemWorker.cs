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
    /// Keep a connection to the ATEM
    /// </summary>
    public class AtemWorker : BackgroundService, INotificationHandler<ConnectionChangeNotify>
    {
        private readonly Switcher _switcher;
        private readonly IMediator _mediator;
        private readonly ILogger _logger;

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
                return Task.CompletedTask;
            }
            
            _logger.LogWarning("Switcher is not connected");
            return StartAsync(cancellationToken);
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
                        _mediator.Publish(new ConnectionChangeNotify { Connected = true }),
                        _mediator.Publish(new InputChangeNotify()),
                        _mediator.Publish(new NextTransitionNotify())
                    );

                    return Task.CompletedTask;
                }
                catch (System.Runtime.InteropServices.COMException)
                {
                    // this indicates a connection could not be made and we should try again
                    Thread.Sleep(1000);
                }
            }

            return Task.CompletedTask;
        }
    }
}
