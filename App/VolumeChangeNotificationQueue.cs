using MediatR;
using Microsoft.Extensions.Logging;
using SwitcherServer.Atem;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SwitcherServer
{
    public class VolumeChangeNotificationQueue : NotificationQueue<VolumeLevelNotify>, IVolumeChangeNotificationQueue
    {
        public VolumeChangeNotificationQueue(ILogger<VolumeChangeNotificationQueue> logger)
        {
            logger.LogInformation("Starting volume change notification queue");
        }
    }
}
