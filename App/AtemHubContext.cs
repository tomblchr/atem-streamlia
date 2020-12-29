using MediatR;
using Microsoft.AspNetCore.SignalR;
using SwitcherServer.Atem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SwitcherServer
{
    public class AtemHubContext : INotificationHandler<InputChangeNotify>
    {
        private readonly Switcher _switcher;
        private readonly IHubContext<AtemHub, IAtemClient> _hub;

        public AtemHubContext(Switcher switcher, IHubContext<AtemHub, IAtemClient> hub)
        {
            _switcher = switcher;
            _hub = hub;
        }

        public async Task SendSceneChange()
        {
            await _hub.Clients.All.ReceiveSceneChange(new SceneDetail(_switcher));
        }

        public Task Handle(InputChangeNotify notification, CancellationToken cancellationToken)
        {
            return SendSceneChange();
        }
    }
}
