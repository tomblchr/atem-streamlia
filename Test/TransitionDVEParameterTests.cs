using NUnit.Framework;
using SwitcherServer.Atem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SwitcherServerTests
{
    [TestFixture]
    public class TransitionDVEParameterTests : Tests
    {
        [Test]
        public void DoSomething()
        {
            var dve = AtemMini.SwitcherDirect
                .GetMixEffectBlocks()
                .First()
                .GetTransitionDVEParameters();

            dve.SetStyle(BMDSwitcherAPI._BMDSwitcherDVETransitionStyle.bmdSwitcherDVETransitionStyleGraphicLogoWipe);
        }
    }
}
