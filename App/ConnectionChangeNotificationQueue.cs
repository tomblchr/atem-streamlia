using SwitcherServer.Atem;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SwitcherServer
{
    public class ConnectionChangeNotificationQueue : IConnectionChangeNotifyQueue
    {
        private SemaphoreSlim _signal = new SemaphoreSlim(0);
        private ConcurrentQueue<ConnectionChangeNotify> _notifications = new ConcurrentQueue<ConnectionChangeNotify>();

        public void QueueNotification(ConnectionChangeNotify notification)
        {
            if (notification == null)
            {
                throw new ArgumentNullException(nameof(notification));
            }

            _notifications.Enqueue(notification);
            _signal.Release();
        }

        public async Task<ConnectionChangeNotify> DequeueAsync(CancellationToken cancellationToken)
        {
            await _signal.WaitAsync(cancellationToken);
            _notifications.TryDequeue(out var notification);
            return notification;
        }
    }
}
