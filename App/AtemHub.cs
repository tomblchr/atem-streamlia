using BMDSwitcherAPI;
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
    /// Methods that client applications will be listening for
    /// </summary>
    public interface IAtemClient
    {
        Task ReceiveSceneChange(SceneDetail detail);

        Task ReceiveConnectConfirmation(string message);

        Task ReceiveConnectionStatus(bool connected);

        Task ReceiveVolume(double volume);

        Task ReceiveInTransition(bool inTransition);

        Task ReceiveIsFadeToBlack(FullyBlackNotify isBlack);
    }

    /// <summary>
    /// Methods the client applications will be calling
    /// </summary>
    public interface IAtemServer
    {
        Task SendSceneChange();

        Task SendProgramChange(long input);

        Task SendPreviewChange(long input);

        Task SendAutoTransition();

        Task SendCutTransition();

        Task SendFadeToBlackTransition();
    }

    public class AtemHub : Hub<IAtemClient>, IAtemServer
    {
        private readonly Switcher _switcher;
        private readonly ILogger _logger;

        public AtemHub(Switcher switcher, ILogger<AtemHub> logger)
        {
            _switcher = switcher;
            _logger = logger;
        }

        public async override Task OnConnectedAsync()
        {
            _logger.LogInformation($"Client Connected... Welcome {Context.ConnectionId}!");
            await base.OnConnectedAsync();
            await SendSceneChange();
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

        public async Task SendProgramChange(long input)
        {
            _switcher.GetMixEffectBlocks().First().Switcher.SetProgramInput(input);
            await Task.CompletedTask;
        }

        public async Task SendPreviewChange(long input)
        {
            _switcher.GetMixEffectBlocks().First().Switcher.SetPreviewInput(input);
            await Task.CompletedTask;
        }

        public async Task SendAutoTransition()
        {
            _switcher.GetMixEffectBlocks().First().Switcher.PerformAutoTransition();
            await Task.CompletedTask;
        }

        public async Task SendCutTransition()
        {
            _switcher.GetMixEffectBlocks().First().Switcher.PerformCut();
            await Task.CompletedTask;
        }

        public async Task SendFadeToBlackTransition()
        {
            _switcher.GetMixEffectBlocks().First().Switcher.PerformFadeToBlack();
            await Task.CompletedTask;
        }
    }
}
