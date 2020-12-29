using BMDSwitcherAPI;
using MediatR;
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

        public IBMDSwitcher SwitcherDirect => _connection.Connect();

        public string GetVideoMode()
        {
            SwitcherDirect.GetVideoMode(out _BMDSwitcherVideoMode name);

            return name.ToString();
        }

        public IEnumerable<Input> GetInputs()
        {
            var result = new List<Input>();

            var inputs = SwitcherDirect.GetInputs();

            inputs.ToList().ForEach(c =>
            {
                c.GetInputId(out long id);
                c.GetLongName(out string name);
                result.Add(new Input(c) { Id = id, Name = name });
            });

            return result;
        }

        public IEnumerable<MixEffectBlock> GetMixEffectBlocks()
        {
            var result = new List<MixEffectBlock>();

            var items = SwitcherDirect.GetMixEffectBlocks();

            items.ToList().ForEach(c =>
            {
                c.GetProgramInput(out long program);
                c.GetPreviewInput(out long preview);
                result.Add(new MixEffectBlock(c, _mediator)
                {
                    ProgramInput = GetInputs().Single(c => c.Id == program),
                    PreviewInput = GetInputs().Single(c => c.Id == preview)
                });
            });

            return result;
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
