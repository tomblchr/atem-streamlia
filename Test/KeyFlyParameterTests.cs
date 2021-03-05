using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace SwitcherServerTests
{
    [TestFixture]
    public class KeyFlyParameterTests : Tests
    {
        [Test]
        public void DoKeys()
        {
            var keyer = AtemMini.GetMixEffectBlocks().First().Keys.First();

            Assert.IsNotNull(keyer);

            keyer.FlyParameters.Switcher.RunToKeyFrame(BMDSwitcherAPI._BMDSwitcherFlyKeyFrame.bmdSwitcherFlyKeyFrameA);

            Thread.Sleep(2000);

            keyer.FlyParameters.Switcher.RunToKeyFrame(BMDSwitcherAPI._BMDSwitcherFlyKeyFrame.bmdSwitcherFlyKeyFrameFull);

            Thread.Sleep(2000);

            keyer.FlyParameters.Switcher.RunToKeyFrame(BMDSwitcherAPI._BMDSwitcherFlyKeyFrame.bmdSwitcherFlyKeyFrameB);

            Thread.Sleep(2000);

            keyer.FlyParameters.Switcher.RunToKeyFrame(BMDSwitcherAPI._BMDSwitcherFlyKeyFrame.bmdSwitcherFlyKeyFrameInfinityCentre);

            Thread.Sleep(2000);

            keyer.FlyParameters.Switcher.RunToKeyFrame(BMDSwitcherAPI._BMDSwitcherFlyKeyFrame.bmdSwitcherFlyKeyFrameB);

            Thread.Sleep(2000);

            keyer.FlyParameters.Switcher.RunToKeyFrame(BMDSwitcherAPI._BMDSwitcherFlyKeyFrame.bmdSwitcherFlyKeyFrameInfinityCentreOfKey);

        }
    }
}
