using BMDSwitcherAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwitcherServer
{
    /// <summary>
    /// Methods the client applications will be calling
    /// </summary>
    public interface IOperateSwitcher
    {
        Task SendSceneChange();

        Task SendProgramChange(long input);

        Task SendPreviewChange(long input);

        Task SendAutoTransition();

        Task SendCutTransition();

        Task SendFadeToBlackTransition();

        Task SendTransitionStyle(_BMDSwitcherTransitionStyle current);
    }
}
