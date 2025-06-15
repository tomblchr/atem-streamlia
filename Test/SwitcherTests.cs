using BMDSwitcherAPI;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using NUnit.Framework;
using SwitcherServer.Atem;
using System.Linq;
using System.Threading;

namespace SwitcherServerTests
{
    public class SwitcherTests : Tests
    {
        [Test]
        public void TestConnect()
        {
            Assert.That(AtemMini is not null);
        }

        [Test]
        public void TestVideoMode()
        {
            // act
            string videoMode = AtemMini.GetVideoMode();

            // assert
            Assert.That("bmdSwitcherVideoMode1080p24" == videoMode);
        }

        [Test]
        public void TestInputs()
        {
            // act
            var inputs = AtemMini.GetInputs();

            // assert
            Assert.That(inputs.Any());

            // info
            inputs.ToList().ForEach(c => Logger.LogInformation(c.Name));
        }

        [Test]
        public void TestMixEffectBlocks()
        {
            // act
            var meb = AtemMini.GetMixEffectBlocks();

            // assert
            Assert.That(meb.Any());

            // info
            meb.ToList().ForEach(c => Logger.LogInformation($"{c.ProgramInput.Name} => {c.PreviewInput.Name}"));
        }

        [Test]
        public void TestDownstreamKeys()
        {
            // act
            var dk = AtemMini.GetDownstreamKeys();

            // assert
            Assert.That(dk.Any());

            // info
            dk.ToList().ForEach(c => Logger.LogInformation($"{c.OnAir}"));
        }

        [Test]
        public void TestKeys()
        {
            // act
            var dk = AtemMini.GetMixEffectBlocks().First().Keys;

            // assert
            Assert.That(dk.Any());

            // info
            dk.ToList().ForEach(c => Logger.LogInformation($"{(int)c.TransitionSelectionMask}-{c.OnAir}"));
        }

        [Test]
        public void TestNextTransition()
        {
            // act
            var transition = AtemMini.GetMixEffectBlocks().First().GetNextTransition();

            // assert
            Assert.That(transition.Keys.Any());

            // info
            Logger.LogInformation($"{transition.IncludeBackground} => {transition.Keys}");
            transition.Keys.ToList().ForEach(c => Logger.LogInformation($"{c.Key}-{c.OnAir}"));
        }

        [Test]
        public void TestTransition()
        {
            // arrange
            var start = AtemMini.GetMixEffectBlocks().First();
            var startProgram = start.ProgramInput.Id;
            var startPreview = start.PreviewInput.Id;

            // act
            AtemMini.PerformAutoTransition();

            // wait for the transition to finish
            Thread.Sleep(3000);

            // assert
            var end = AtemMini.GetMixEffectBlocks().First();
            Assert.That(startProgram == end.PreviewInput.Id);
            Assert.That(startPreview == end.ProgramInput.Id);
        }
        
        [Test]
        public void TestChangeProgramInput()
        {

        }

        [Test]
        public void TestChangePreviewInput()
        {

        }
    }
}