using MediatR;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SwitcherServer
{
    public abstract class NotificationQueue<T> : INotificationQueue<T> where T : INotification
    {
        const int MAX_QUEUE_LENGTH = 1000;
        private SemaphoreSlim _signal = new SemaphoreSlim(0);
        private ConcurrentQueue<T> _notifications = new ConcurrentQueue<T>();

        public void QueueNotification(T notification)
        {
            if (notification == null)
            {
                throw new ArgumentNullException(nameof(notification));
            }
 
            try
            {
                RestrictQueueLength();
                _notifications.Enqueue(notification);
            }
            finally
            {
                _signal.Release();
            }
        }

        public async Task<T> DequeueAsync(CancellationToken cancellationToken)
        {
            await _signal.WaitAsync(cancellationToken);
            _notifications.TryDequeue(out var notification);
            return notification;
        }

        void RestrictQueueLength()
        {
            while (_notifications.Count >= MAX_QUEUE_LENGTH)
            {
                if (!_notifications.TryDequeue(out var _))
                {
                    break;
                }
            }
        }
    }
}
