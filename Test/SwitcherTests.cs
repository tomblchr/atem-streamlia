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
        readonly Switcher _switcher;

        public SwitcherTests()
        {
            _switcher = new SwitcherBuilder(new MockMediator())
                .Build();
        }

        [Test]
        public void TestConnect()
        {
            Assert.IsNotNull(_switcher);
        }

        [Test]
        public void TestVideoMode()
        {
            // act
            string videoMode = _switcher.GetVideoMode();

            // assert
            Assert.AreEqual("bmdSwitcherVideoMode1080p2997", videoMode);
        }

        [Test]
        public void TestInputs()
        {
            // act
            var inputs = _switcher.GetInputs();

            // assert
            Assert.IsNotEmpty(inputs);

            // info
            inputs.ToList().ForEach(c => Logger.LogInformation(c.Name));
        }

        [Test]
        public void TestMixEffectBlocks()
        {
            // act
            var meb = _switcher.GetMixEffectBlocks();

            // assert
            Assert.IsNotEmpty(meb);

            // info
            meb.ToList().ForEach(c => Logger.LogInformation($"{c.ProgramInput.Name} => {c.PreviewInput.Name}"));
        }

        [Test]
        public void TestTransition()
        {
            // arrange
            var start = _switcher.GetMixEffectBlocks().First();
            var startProgram = start.ProgramInput.Id;
            var startPreview = start.PreviewInput.Id;

            // act
            _switcher.PerformAutoTransition();

            // wait for the transition to finish
            Thread.Sleep(3000);

            // assert
            var end = _switcher.GetMixEffectBlocks().First();
            Assert.AreEqual(startProgram, end.PreviewInput.Id);
            Assert.AreEqual(startPreview, end.ProgramInput.Id);
        }
    }
}