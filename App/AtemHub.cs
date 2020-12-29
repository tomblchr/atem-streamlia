using BMDSwitcherAPI;
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
    /// <summary>
    /// Methods that client applications will be listening for
    /// </summary>
    public interface IAtemClient
    {
        Task ReceiveSceneChange(SceneDetail detail);

        Task ReceiveConnectConfirmation(string message);
    }

    /// <summary>
    /// Methods the client applications will be calling
    /// </summary>
    public interface IAtemServer
    {
        Task SendSceneChange();
    }

    public class AtemHub : Hub<IAtemClient>, IAtemServer
    {
        private readonly Switcher _switcher;

        public AtemHub(Switcher switcher)
        {
            _switcher = switcher;
        }

        public async override Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }

        public async Task Subscribe(Guid id)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, id.ToString());
            await Clients.Caller.ReceiveConnectConfirmation("Connected!");
        }

        public async Task SendSceneChange()
        {
            await Clients.All.ReceiveSceneChange(new SceneDetail(_switcher));
        }
    }
}
