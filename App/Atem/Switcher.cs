using BMDSwitcherAPI;
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
        }

        public IBMDSwitcher SwitcherDirect
        {
            get
            {
                return _connection.Connect();
            }
        }

        public void Reset(string ipaddress = "")
        {
            _inputs = null;
            _mixEffectBlocks = null;
            _fairlightAudioMixer = null;
            _downstreamKeys = null;
            _connection.Disconnect();
            var switcher = _connection.Connect(ipaddress);

            if (switcher != null)
            {
                GetInputs();
                GetMixEffectBlocks();
                GetFairlightAudioMixer();
                GetDownstreamKeys();
                GetMacros();
            }
        }

        public bool IsConnected
        {
            get
            {
                return _connection.IsConnected;
            }
        }

        public string GetProductName()
        {
            SwitcherDirect.GetProductName(out string productName);
            return productName;
        }

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

        private IBMDSwitcherMacroPool _macroPool;
        /// <summary>
        /// Macros with a name in the system
        /// </summary>
        /// <remarks>
        /// Unlike other aspects that are fixed parts of the hardware, inputs, mix blocks etc., macros are
        /// dynamic and are retrieved on every call to this function.
        /// </remarks>
        /// <returns></returns>
        public IEnumerable<Macro> GetMacros()
        {
            var result = new List<Macro>();
            if (_macroPool == null)
            {
                _macroPool = SwitcherDirect.GetMacroPool();
                _macroPool.AddCallback(new MacroPoolCallback(_mediator));
            }
            _macroPool.GetMaxCount(out uint maxCount);
            for (uint i = 0; i < maxCount; i++)
            {
                _macroPool.GetName(i, out string name);
                _macroPool.IsValid(i, out int valid);
                if (valid == 1 && !string.IsNullOrEmpty(name))
                {
                    _macroPool.GetDescription(i, out string description);
                    result.Add(new Macro { Id = i, Name = name, Description = description });
                }
            }
            return result;
        }

        public IBMDSwitcherMacroControl MacroControl => SwitcherDirect.GetMacroControl();

        public void PerformAutoTransition()
        {
            GetMixEffectBlocks()
                .First()
                .Switcher
                .PerformAutoTransition();
        }
    }
}
