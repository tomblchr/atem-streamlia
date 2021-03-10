using BMDSwitcherAPI;
using SwitcherServer.Atem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwitcherServer
{
    /// <summary>
    /// Methods that client applications will be listening for
    /// </summary>
    public interface IClientNotifications
    {
        Task ReceiveSceneChange(SceneDetail detail);

        Task ReceiveConnectConfirmation(string message);

        Task ReceiveConnectionStatus(bool connected);

        Task ReceiveVolume(MasterOutLevelNotify volume);

        Task ReceiveInTransition(bool inTransition);

        Task ReceiveIsFadeToBlack(FullyBlackNotify isBlack);

        Task ReceiveTransitionStyle(_BMDSwitcherTransitionStyle style);

        Task ReceiveTransitionPosition(TransitionPositionNotify position);

        Task ReceiveDownstreamKeyInTransition(bool inTransition);

        Task ReceiveNextTransition(NextTransition notification);

        Task ReceiveKeyFlyParameters(KeyFlyParametersNotify notification);

        Task ReceiveMacros(IEnumerable<Macro> macros);
    }
}
