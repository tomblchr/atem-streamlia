using Microsoft.Extensions.Logging;
using NUnit.Framework;
using SwitcherServer.Atem;
using System;
using System.Collections.Generic;
using System.Linq;
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

        [Test]
        public void CanGetFairlightAudioMixerInputs()
        {
            // act
            var result = AtemMini.GetFairlightAudioMixer().Inputs;

            // assert
            Assert.IsNotNull(result);
            Assert.GreaterOrEqual(result.Count(), 0);
        }

        [Test]
        public void CanGetFairlightAudioMixerInputSources()
        {
            // act
            var result = AtemMini.GetFairlightAudioMixer().Inputs.SelectMany(c => c.Sources);

            // assert
            Assert.IsNotNull(result);
            Assert.GreaterOrEqual(result.Count(), 0);
            result.ToList().ForEach(c => { Logger.LogInformation($"{c.Input.Id}/{c.Id}=> Active: {c.IsActive},{c.MixOption},{c.InputGain},{c.FaderGain}"); });
        }
    }
}
