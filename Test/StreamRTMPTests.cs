using BMDSwitcherAPI;
using MediatR;
using NUnit.Framework;
using SwitcherServer.Atem;
using System;
using System.Collections.Generic;
using System.Text;

namespace SwitcherServerTests
{
    [TestFixture]
    public class StreamRTMPTests : Tests
    {
        IBMDSwitcherStreamRTMP _stream;
        IMediator _mediator;

        public StreamRTMPTests() : base()
        {
            _stream = AtemMini.SwitcherDirect.GetStreamRTMP() ?? new Mock.MockStream();
            _mediator = new MockMediator();
        }

        [Test]
        public void TestStreamingNotSupported()
        {
            // arrange
            var s = new StreamRTMP(null, _mediator);

            // act
            s.Start();
            s.Stop();

            // assert
            Assert.That(!s.IsStreamingSupported);
        }

        [Test]
        public void TestStartStream()
        {
            // arrange
            var s = new StreamRTMP(_stream, _mediator);

            // act
            s.Start();

            // assert
            Assert.That(s.IsStreaming);
        }

        [Test]
        public void TestStopStream()
        {
            // arrange
            var s = new StreamRTMP(_stream, _mediator);

            // act
            s.Stop();

            // assert
            Assert.That(!s.IsStreaming);
        }
    }
}
