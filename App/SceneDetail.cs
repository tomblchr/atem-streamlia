using SwitcherServer.Atem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwitcherServer
{
    /// <summary>
    /// The current state of the switcher.
    /// </summary>
    public class SceneDetail
    {
        public SceneDetail(Switcher switcher)
        {
            Inputs = switcher
                .GetInputs()
                .ToArray();

            var i = switcher
                .GetMixEffectBlocks()
                .ToList()
                .First();
            
            Program = i.ProgramInput.Id;
            Preview = i.PreviewInput.Id;
        }

        public long Program { get; private set; }

        public long Preview { get; private set; }

        public Input[] Inputs { get; private set; }
    }
}
