using Microsoft.Extensions.Logging;
using NUnit.Framework;
using SwitcherServer;
using System;
using System.Collections.Generic;
using System.Text;

namespace SwitcherServerTests
{
    [TestFixture]
    public class NetworkInspectorTests : Tests
    {
        [Test]
        public void FindIPAddress()
        {
            var ip = NetworkInspector.GetLocalIPv4(System.Net.NetworkInformation.NetworkInterfaceType.Ethernet);

            Logger.LogInformation(ip);

            Assert.That(NetworkInspector.UNKNOWN_IP != ip);
        }
    }
}
