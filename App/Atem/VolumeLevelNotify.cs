using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwitcherServer.Atem
{
    public class VolumeLevelNotify : INotification
    {
        public VolumeLevelNotify(long inputId, long sourceId, uint numLevels, double[] levels, uint numPeakLevels, double[] peakLevels)
        {
            Levels = levels;
            Peaks = peakLevels;
            InputId = inputId;
            SourceId = sourceId;
        }

        public long InputId { get; private set; }

        public long SourceId { get; private set; }

        public double[] Levels { get; }

        public double[] Peaks { get; }
    }
}
