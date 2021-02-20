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
        Task SendConnect(string ipaddress);

        Task SendSceneChange();

        Task SendProgramChange(long input);

        Task SendPreviewChange(long input);

        Task SendAutoTransition();

        Task SendCutTransition();

        Task SendFadeToBlackTransition();

        Task SendTransitionStyle(_BMDSwitcherTransitionStyle current);

        Task SendDownstreamKeyOnAir(bool onAir);

        Task SendDownstreamKeyTie(bool tie);

        Task SendDownstreamKeyAutoTransition();

        Task SendKeyOnAir(int key, bool onAir);

        Task SendKeyIncludedInTransition(int key, bool included);

        Task SendNextTransitionBackground(bool included);
    }
}
