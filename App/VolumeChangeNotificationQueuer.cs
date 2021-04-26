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
    public class VolumeChangeNotificationQueuer : INotificationHandler<MasterOutLevelNotify>, INotificationHandler<VolumeLevelNotify>
    {
        private readonly IVolumeChangeNotificationQueue _queue;
        private readonly ILogger<VolumeChangeNotificationQueuer> _logger;

        public VolumeChangeNotificationQueuer(IVolumeChangeNotificationQueue queue, ILogger<VolumeChangeNotificationQueuer> logger)
        {
            _queue = queue;
            _logger = logger;
        }

        public Task Handle(MasterOutLevelNotify notification, CancellationToken cancellationToken)
        {
            return Handle((VolumeLevelNotify)notification, cancellationToken);
        }

        public Task Handle(VolumeLevelNotify notification, CancellationToken cancellationToken)
        {

            _queue.QueueNotification(notification);

            return Task.CompletedTask;
        }
    }
}
