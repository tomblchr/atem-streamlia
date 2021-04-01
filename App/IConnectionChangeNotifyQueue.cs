using SwitcherServer.Atem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SwitcherServer
{
    public interface IConnectionChangeNotifyQueue
    {
        void QueueNotification(ConnectionChangeNotify notification);

        Task<ConnectionChangeNotify> DequeueAsync(CancellationToken cancellationToken);
    }
}
