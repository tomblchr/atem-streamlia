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
    public class AtemHubContext : 
          INotificationHandler<InputChangeNotify>
        , INotificationHandler<MasterOutLevelNotify>
        , INotificationHandler<InTransitionNotify>
        , INotificationHandler<FullyBlackNotify>
        , INotificationHandler<TransitionStyleNotify>
        , INotificationHandler<TransitionPositionNotify>
        , INotificationHandler<DownstreamKeyAutoTransitionNotify>
        , INotificationHandler<NextTransitionNotify>
        , INotificationHandler<SwitcherMessageNotify>
        , INotificationHandler<KeyFlyParametersNotify>
    {
        private readonly Switcher _switcher;
        private readonly IHubContext<AtemHub, IClientNotifications> _hub;
        private readonly ILogger<AtemHubContext> _logger;

        public AtemHubContext(Switcher switcher, IHubContext<AtemHub, IClientNotifications> hub, ILogger<AtemHubContext> logger)
        {
            _switcher = switcher;
            _hub = hub;
            _logger = logger;
        }

        public async Task Handle(InputChangeNotify notification, CancellationToken cancellationToken)
        {
            _logger.LogDebug("Send 'input change' notification");
            await _hub.Clients.All.ReceiveSceneChange(new SceneDetail(_switcher));
        }

        public async Task Handle(MasterOutLevelNotify notification, CancellationToken token)
        {
            // there are lots of these
            // _logger.LogDebug($"Master Out Level: {string.Join(',', notification.Levels)}");
            // TODO: rework this to put this logic it into the correct place
            for (int i = 0; i < notification.Levels.Length; i++)
            {
                if (notification.Levels[i] == double.NegativeInfinity)
                {
                    notification.Levels[i] = -120;
                }
            }
            for (int i = 0; i < notification.Peaks.Length; i++)
            {
                if (notification.Peaks[i] == double.NegativeInfinity)
                {
                    notification.Peaks[i] = -120;
                }
            }

            await _hub.Clients.All.ReceiveVolume(notification);
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

        public async Task Handle(TransitionStyleNotify notification, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Transition Style: {notification.Current}");
            await _hub.Clients.All.ReceiveTransitionStyle(notification.Current);
        }

        public async Task Handle(TransitionPositionNotify notification, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Transition Position: {notification.Position}, {notification.FramesRemaining}");
            await _hub.Clients.All.ReceiveTransitionPosition(notification);
        }

        public async Task Handle(DownstreamKeyAutoTransitionNotify notification, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"DownstreamKey In Transition: {notification.IsTransitioning}");
            await _hub.Clients.All.ReceiveDownstreamKeyInTransition(notification.IsTransitioning);
        }

        public async Task Handle(NextTransitionNotify notification, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Next Transition");
            await _hub.Clients.All.ReceiveNextTransition(_switcher.GetMixEffectBlocks().First().GetNextTransition());
        }

        public async Task Handle(SwitcherMessageNotify notification, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"{notification.Message}");

            await _hub.Clients.All.ReceiveSceneChange(new SceneDetail(_switcher));
        }

        public async Task Handle(KeyFlyParametersNotify notification, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Key frame {notification.Destination} is " + (notification.IsRunning ? "" : " NOT ") + " in motion");
            await _hub.Clients.All.ReceiveKeyFlyParameters(notification);
        }
    }
}
