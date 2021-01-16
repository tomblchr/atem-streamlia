using NUnit.Framework;
using SwitcherServer.Atem;
using System;
using System.Collections.Generic;
using System.Text;

namespace SwitcherServerTests
{
    [TestFixture]
    public class FairlightAudioMixerTests
    {
        readonly Switcher _switcher;

        public FairlightAudioMixerTests()
        {
            _switcher = new SwitcherBuilder(new MockMediator())
                .Build();
        }

        [Test]
        public void CanGetFairlightAudioMixerDirect()
        {
            // act
            var result = _switcher.SwitcherDirect.GetFairlightAudioMixer();

            // assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void CanGetFairlightAudioMixerProperties()
        {
            // act
            var result = _switcher.GetFairlightAudioMixer();

            // assert
            Assert.IsNotNull(result);
            Assert.GreaterOrEqual(0, result.MasterOutFaderGain);
        }
    }
}
