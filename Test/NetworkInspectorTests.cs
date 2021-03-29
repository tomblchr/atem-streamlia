﻿using Microsoft.Extensions.Logging;
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
            var ip = NetworkInspector.GetLocalIPv4(System.Net.NetworkInformation.NetworkInterfaceType.Wireless80211);

            Logger.LogInformation(ip);

            Assert.AreNotEqual(NetworkInspector.UNKNOWN_IP, ip);
        }
    }
}
