using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using SwitcherServer.Atem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SwitcherServer
{
    /// <summary>
    /// Communicate service-to-atem connection changes to the clients
    /// </summary>
    public class AtemHubConnectionCommunicator : INotificationHandler<ConnectionChangeNotify>
    {
        private readonly Switcher _switcher;
        private readonly IHubContext<AtemHub, IClientNotifications> _hub;
        private readonly ILogger<AtemHubConnectionCommunicator> _logger;

        public AtemHubConnectionCommunicator(Switcher switcher, IHubContext<AtemHub, IClientNotifications> hub, ILogger<AtemHubConnectionCommunicator> logger)
        {
            _switcher = switcher;
            _hub = hub;
            _logger = logger;
        }

        public async Task Handle(ConnectionChangeNotify notification, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Send 'connection status' notification. Connected: {notification.Connected}");

            await _hub.Clients.All.ReceiveConnectionStatus(notification.Connected);

            if (notification.Connected)
            {
                _switcher.GetInputs();
                _switcher.GetMacros();                
                _switcher.GetMixEffectBlocks();
                _switcher.GetDownstreamKeys();
                _switcher.GetFairlightAudioMixer();

                Task.WaitAll(
                    _hub.Clients.All.ReceiveSceneChange(new SceneDetail(_switcher)),
                    _hub.Clients.All.ReceiveNextTransition(_switcher.GetMixEffectBlocks().First().GetNextTransition()),
                    _hub.Clients.All.ReceiveMacros(_switcher.GetMacros())
                );
            }
        }
    }
}
