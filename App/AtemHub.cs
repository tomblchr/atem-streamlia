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
    public class AtemHub : Hub<IClientNotifications>, IOperateSwitcher
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

        public async override Task OnDisconnectedAsync(Exception exception)
        {
            _logger.LogError($"Client Disconnected... {exception}!");
            await base.OnDisconnectedAsync(exception);

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

        public async Task SendTransitionStyle(_BMDSwitcherTransitionStyle current)
        {
            _switcher.GetMixEffectBlocks().First().Switcher.GetTransitionParameters().SetNextTransitionStyle(current);
            await Task.CompletedTask;
        }

        public async Task SendDownstreamKeyOnAir(bool onAir)
        {
            _switcher.GetDownstreamKeys().First().OnAir = onAir;
            await Task.CompletedTask;
        }

        public async Task SendDownstreamKeyTie(bool tie)
        {
            _switcher.GetDownstreamKeys().First().Tie = tie;
            await Task.CompletedTask;
        }

        public async Task SendDownstreamKeyAutoTransition()
        {
            _switcher.GetDownstreamKeys().First().DoSomething();
            await Task.CompletedTask;
        }

        public async Task SendKeyOnAir(int key, bool onAir)
        {
            var keyer = _switcher.GetMixEffectBlocks().First()
                .Keys
                .Where(c => (int)c.TransitionSelectionMask == key)
                .Single();
            
            keyer.OnAir = onAir;
            
            await Task.CompletedTask;
        }

        public async Task SendKeyIncludedInTransition(int key, bool included)
        {
            var keyer = _switcher.GetMixEffectBlocks().First();

            keyer.SetIncludeInNextTrasition((_BMDSwitcherTransitionSelection)key, included);

            await Task.CompletedTask;
        }

        public async Task SendNextTransitionBackground(bool included)
        {
            var keyer = _switcher.GetMixEffectBlocks().First();

            keyer.SetIncludeInNextTrasition(_BMDSwitcherTransitionSelection.bmdSwitcherTransitionSelectionBackground, included);

            await Task.CompletedTask;
        }
    }
}
