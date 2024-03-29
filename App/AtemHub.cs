﻿using BMDSwitcherAPI;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using SwitcherServer.Atem;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace SwitcherServer
{
    public class AtemHub : Hub<IClientNotifications>, IOperateSwitcher
    {
        private readonly Switcher _switcher;
        private readonly IVolumeChangeNotificationQueue _volume;
        private readonly ILogger _logger;

        private static SemaphoreSlim _lock = new SemaphoreSlim(1, 1);
        private static string _livestreamPreviewUrl;

        public AtemHub(Switcher switcher, IVolumeChangeNotificationQueue volume, ILogger<AtemHub> logger)
        {
            _switcher = switcher;
            _volume = volume;
            _logger = logger;
        }

        /// <summary>
        /// Connect this application to a switcher at the given address
        /// </summary>
        /// <param name="ipaddress"></param>
        /// <returns></returns>
        public async Task SendConnect(string ipaddress)
        {
            _switcher.Reset(ipaddress);
            await Clients.Caller.ReceiveConnectionStatus(_switcher.IsConnected);
            await Task.CompletedTask;
        }

        public async Task SendStartStreaming()
        {
            _switcher.GetStreamRTMP().Start();
            await Task.CompletedTask;
        }

        public async Task SendStopStreaming()
        {
            _switcher.GetStreamRTMP().Stop();
            await Task.CompletedTask;
        }

        public async override Task OnConnectedAsync()
        {
            _logger.LogInformation($"Client Connected... Welcome {Context.ConnectionId}!");

            await base.OnConnectedAsync();

            Task.WaitAll(
                Clients.Caller.ReceiveConnectConfirmation("Connected to server!"),
                Clients.Caller.ReceiveConnectionStatus(_switcher.IsConnected)
            );

            if (_switcher.IsConnected)
            {
                Task.WaitAll(
                    Clients.Caller.ReceiveSceneChange(new SceneDetail(_switcher)),
                    Clients.Caller.ReceiveLivestreamPreviewUrl(_livestreamPreviewUrl),
                    Clients.Caller.ReceiveNextTransition(_switcher.GetMixEffectBlocks().First().GetNextTransition()),
                    Clients.Caller.ReceiveTransitionStyle(_switcher.GetMixEffectBlocks().First().NextTransitionStyle),
                    Clients.Caller.ReceiveStreamingStatus(_switcher.GetStreamRTMP().IsStreaming),
                    Clients.Caller.ReceiveMacros(_switcher.GetMacros())
                );
            }
        }

        public async Task SendHealthCheckRequest()
        {
            await Clients.Caller.ReceiveConnectConfirmation("Connected to server!");
            await Clients.Caller.ReceiveConnectionStatus(_switcher.IsConnected);
        }

        public async override Task OnDisconnectedAsync(Exception exception)
        {
            if (exception == null)
            {
                _logger.LogInformation($"Client Disconnected... Goodbye {Context.ConnectionId}!");
            }
            else
            {
                _logger.LogError($"Client Disconnected... Goodbye {Context.ConnectionId} - {exception.Message}!");
            }
            await base.OnDisconnectedAsync(exception);

        }

        public async Task Subscribe(Guid id)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, id.ToString());
            await Clients.Caller.ReceiveConnectConfirmation("Connected to server!");
            await Clients.Caller.ReceiveConnectionStatus(_switcher.IsConnected);
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

        public async Task SendRunKeyFrame(_BMDSwitcherFlyKeyFrame flyKeyFrame)
        {
            _switcher.GetMixEffectBlocks().First()
                .Keys.First()
                .FlyParameters
                .Switcher
                .RunToKeyFrame(flyKeyFrame);

            await Task.CompletedTask;
        }

        public async Task SendMacros()
        {
            await Clients.Caller.ReceiveMacros(_switcher.GetMacros());
        }

        public async Task SendRunMacro(uint id)
        {
            _switcher.MacroControl.Run(id);

            await Task.CompletedTask;
        }

        public async Task SendLivestreamPreviewUrl(string value)
        {
            _lock.Wait();
            try
            {
                _livestreamPreviewUrl = value;

                // relay the livestream preview URL to all clients, including the caller
                await Clients.All.ReceiveLivestreamPreviewUrl(_livestreamPreviewUrl);
                await Task.CompletedTask;
            }
            finally
            {
                _lock.Release();
            }
        }

        /// <summary>
        /// Read the volume notification queue and stream details to a client
        /// </summary>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        public async IAsyncEnumerable<VolumeLevelNotify> ReceiveVolumeChange([EnumeratorCancellation] CancellationToken cancellation)
        {
            _logger.LogDebug($"Start volume notification streaming for client {Context.ConnectionId}");
            try
            {
                while (!cancellation.IsCancellationRequested)
                {
                    cancellation.ThrowIfCancellationRequested();
                    var v = await _volume.DequeueAsync(cancellation);
                    yield return v.WithoutInfinity();
                }
            }
            finally
            {
                _logger.LogDebug($"Ending volume notification streaming for client {Context.ConnectionId}");
            }
        }
    }
}
