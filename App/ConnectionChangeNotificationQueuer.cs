using MediatR;
using Microsoft.Extensions.Logging;
using SwitcherServer.Atem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SwitcherServer
{
    /// <summary>
    /// Put connection change notifications onto a queue.
    /// </summary>
    public class ConnectionChangeNotificationQueuer : INotificationHandler<ConnectionChangeNotify>
    {
        private readonly IConnectionChangeNotifyQueue _queue;
        private readonly ILogger _logger;

        public ConnectionChangeNotificationQueuer(IConnectionChangeNotifyQueue queue, ILogger<ConnectionChangeNotificationQueuer> logger)
        {
            _queue = queue;
            _logger = logger;
        }

        public Task Handle(ConnectionChangeNotify notification, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Adding connection change 'connected:{notification.Connected}' to the queue");
            _queue.QueueNotification(notification);
            return Task.CompletedTask;
        }
    }
}
