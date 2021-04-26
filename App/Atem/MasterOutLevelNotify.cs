using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwitcherServer.Atem
{
    public class MasterOutLevelNotify : VolumeLevelNotify, INotification
    {
        public MasterOutLevelNotify(uint numLevels, double[] levels, uint numPeakLevels, double[] peakLevels) 
            : base(0, 0, numLevels, levels, numPeakLevels, peakLevels)
        {

        }
    }
}
