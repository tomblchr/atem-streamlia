using SwitcherServer.Atem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwitcherServer
{
    public interface IVolumeChangeNotificationQueue : INotificationQueue<VolumeLevelNotify>
    {

    }
}
