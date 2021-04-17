using MediatR;
using SwitcherServer.Atem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SwitcherServer
{
    public class VolumeChangeNotificationQueuer : INotificationHandler<MasterOutLevelNotify>
    {
        private readonly IVolumeChangeNotificationQueue _queue;

        public VolumeChangeNotificationQueuer(IVolumeChangeNotificationQueue queue)
        {
            _queue = queue;
        }

        public Task Handle(MasterOutLevelNotify notification, CancellationToken cancellationToken)
        {
            _queue.QueueNotification(notification);

            return Task.CompletedTask;
        }
    }
}
