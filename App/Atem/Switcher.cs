﻿using BMDSwitcherAPI;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwitcherServer.Atem
{
    public class Switcher
    {
        readonly ISwitcherConnection _connection;
        private readonly IMediator _mediator;

        internal Switcher(ISwitcherConnection connection, IMediator mediator)
        {
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

            GetMixEffectBlocks();
            GetFairlightAudioMixer();
        }

        public IBMDSwitcher SwitcherDirect => _connection.Connect();

        public string GetVideoMode()
        {
            SwitcherDirect.GetVideoMode(out _BMDSwitcherVideoMode name);

            return name.ToString();
        }

        private IEnumerable<Input> _inputs;
        public IEnumerable<Input> GetInputs()
        {
            if (_inputs == null)
            {
                var inputs = SwitcherDirect.GetInputs();
                _inputs = new List<Input>(inputs.ToList().Select(c => new Input(c)));
            }
            return _inputs;
        }

        public IEnumerable<Input> GetSourceInputs()
        {
            var sourceInputs = new [] { 
                _BMDSwitcherPortType.bmdSwitcherPortTypeBlack,
                _BMDSwitcherPortType.bmdSwitcherPortTypeExternal
            };
            return GetInputs().Where(c => sourceInputs.Contains(c.InputType));
        }

        private IEnumerable<MixEffectBlock> _mixEffectBlocks;
        public IEnumerable<MixEffectBlock> GetMixEffectBlocks()
        {
            if (_mixEffectBlocks == null)
            {
                var items = SwitcherDirect.GetMixEffectBlocks();
                _mixEffectBlocks = new List<MixEffectBlock>(items
                    .ToList()
                    .Select(c => new MixEffectBlock(this, c, _mediator)));
            }

            return _mixEffectBlocks;
        }

        private FairlightAudioMixer _fairlightAudioMixer;
        public FairlightAudioMixer GetFairlightAudioMixer()
        {
            if (_fairlightAudioMixer == null)
            {
                _fairlightAudioMixer = new FairlightAudioMixer(SwitcherDirect.GetFairlightAudioMixer(), _mediator);
            }
            return _fairlightAudioMixer;
        }

        private IEnumerable<DownstreamKey> _downstreamKeys;
        public IEnumerable<DownstreamKey> GetDownstreamKeys()
        {
            if (_downstreamKeys == null)
            {
                var items = SwitcherDirect.GetDownstreamKeys();
                _downstreamKeys = new List<DownstreamKey>(items.ToList().Select(c => new DownstreamKey(c, _mediator)));
            }
            return _downstreamKeys;
        }

        public void PerformAutoTransition()
        {
            GetMixEffectBlocks()
                .First()
                .Switcher
                .PerformAutoTransition();
        }
    }
}
