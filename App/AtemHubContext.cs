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
    /// Relay notifications from the application, usually from callbacks
    /// triggered by the switcher, to the connected client applications.
    /// </summary>
    public class AtemHubContext : INotificationHandler<InputChangeNotify>
    {
        private readonly Switcher _switcher;
        private readonly IHubContext<AtemHub, IAtemClient> _hub;
        private readonly ILogger<AtemHubContext> _logger;

        public AtemHubContext(Switcher switcher, IHubContext<AtemHub, IAtemClient> hub, ILogger<AtemHubContext> logger)
        {
            _switcher = switcher;
            _hub = hub;
            _logger = logger;
        }

        public async Task Handle(ConnectionChangeNotify notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Send 'connection status' notification");
            await _hub.Clients.All.ReceiveConnectionStatus(notification.Connected);
        }

        public async Task Handle(InputChangeNotify notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Send 'input change' notification");
            await _hub.Clients.All.ReceiveSceneChange(new SceneDetail(_switcher));
        }
    }
}
