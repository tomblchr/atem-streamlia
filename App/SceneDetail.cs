using SwitcherServer.Atem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwitcherServer
{
    public class SceneDetail
    {
        public SceneDetail(Switcher switcher)
        {
            var i = switcher
                .GetMixEffectBlocks()
                .ToList()
                .First();
            
            Program = i.ProgramInput.Name;
            Preview = i.PreviewInput.Name;
        }

        public string Program { get; private set; }

        public string Preview { get; private set; }
    }
}
