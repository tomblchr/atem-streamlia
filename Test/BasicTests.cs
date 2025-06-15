using BMDSwitcherAPI;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Engine.TesthostProtocol;
using NUnit.Framework;
using SwitcherServer.Atem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwitcherServerTests
{
    [TestFixture]
    public class BasicTests : Tests
    {
        [Test]
        public void TheBasics()
        {
            Assert.That(AtemMini is not null);
            Assert.That(AtemMini.IsConnected);

            // Log info about the switcher under test
            Logger.LogInformation($"Video Mode: {AtemMini.GetVideoMode()}");
            Logger.LogInformation($"Product Name: {AtemMini.GetProductName()}");
            AtemMini.SwitcherDirect.GetPowerStatus(out _BMDSwitcherPowerStatus powerStatus);
            Logger.LogInformation($"Power Status: {powerStatus}");
            Logger.LogInformation(typeof(IBMDSwitcher).Assembly.FullName);
            AtemMini.SwitcherDirect.GetMediaPool().GetStills(out IBMDSwitcherStills stills);
            stills.GetCount(out uint stillCount);
            Logger.LogInformation($"Still Count: {stillCount}");
        }
    }
}
