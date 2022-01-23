using BMDSwitcherAPI;
using MediatR;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using SwitcherServer.Atem;
using System;
using System.Collections.Generic;
using System.Text;

namespace SwitcherServerTests
{
    [TestFixture]
    public class RecordAVTests : Tests
    {
        IBMDSwitcherRecordAV _recorder;
        IMediator _mediator;

        public RecordAVTests() : base()
        {
            _recorder = this.AtemMini.SwitcherDirect.GetRecordAV();
            _mediator = new MockMediator();
        }

        [Test]
        public void DoSomething()
        {
            _recorder.IsRecording(out int recording);

            base.Logger.LogInformation($"Recording: {recording}");
        }
    }
}
