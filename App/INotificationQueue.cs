using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SwitcherServer
{
    public interface INotificationQueue<T> where T : INotification
    {
        void QueueNotification(T notification);

        Task<T> DequeueAsync(CancellationToken cancellationToken);
    }
}
