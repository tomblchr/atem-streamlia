using NUnit.Framework;
using SwitcherServer.Atem;
using System;
using System.Collections.Generic;
using System.Text;

namespace SwitcherServerTests
{
    [TestFixture]
    public class FairlightAudioMixerTests : Tests
    {
        [Test]
        public void CanGetFairlightAudioMixerDirect()
        {
            // act
            var result = AtemMini.SwitcherDirect.GetFairlightAudioMixer();

            // assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void CanGetFairlightAudioMixerProperties()
        {
            // act
            var result = AtemMini.GetFairlightAudioMixer();

            // assert
            Assert.IsNotNull(result);
            Assert.GreaterOrEqual(0, result.MasterOutFaderGain);
        }
    }
}
