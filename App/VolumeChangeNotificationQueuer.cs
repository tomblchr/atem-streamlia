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
            if (notification.InputId == 0)
            {
                _logger.LogDebug($"Master Volume Out Level: {string.Join(',', notification.Levels)}:{string.Join(',', notification.Peaks)}");
            }
            else
            {
                _logger.LogDebug($"Input Volume Level ({notification.InputId},{notification.SourceId}): {string.Join(',', notification.Levels)}:{string.Join(',', notification.Peaks)}");
            }

            _queue.QueueNotification(notification);

            return Task.CompletedTask;
        }
    }
}
