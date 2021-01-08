﻿using MediatR;
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
    public class AtemHubContext : 
          INotificationHandler<InputChangeNotify>
        , INotificationHandler<MasterOutLevelNotify>
        , INotificationHandler<InTransitionNotify>
        , INotificationHandler<FullyBlackNotify>
//        , INotificationHandler<ConnectionChangeNotify>
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

        public async Task Handle(MasterOutLevelNotify notification, CancellationToken token)
        {
            _logger.LogInformation($"Master Out Level: {notification.NumLevels}-{notification.Levels}");
            await _hub.Clients.All.ReceiveVolume(notification.Levels);
        }

        public async Task Handle(InTransitionNotify notification, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"In Transition: {notification.InTransition}");
            await _hub.Clients.All.ReceiveInTransition(notification.InTransition);
        }

        public async Task Handle(FullyBlackNotify notification, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Is fully black: {notification.IsFullyBlack}. Is in transition: {notification.IsInTransition}");
            await _hub.Clients.All.ReceiveIsFadeToBlack(notification);
        }
    }
}
