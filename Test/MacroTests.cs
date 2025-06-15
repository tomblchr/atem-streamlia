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
    public class MacroTests : Tests
    {
        [Test]
        public void ShowMacros()
        {
            // arrange
            var pool = AtemMini.SwitcherDirect.GetMacroPool();

            // act
            pool.GetMaxCount(out uint maxCount);

            Logger.LogInformation($"Number of macros: {maxCount}");

            for (uint i = 0; i < maxCount; i++)
            {
                pool.GetName(i, out string name);
                pool.GetDescription(i, out string description);
                if (!string.IsNullOrEmpty(name))
                {
                    Logger.LogInformation($"Macro: {i}/{name}-{description}");
                }
            }
            // assert

        }

        [Test]
        public void CanGetMacrosFromSwitcher()
        {
            // arrange
            var pool = AtemMini.GetMacros();

            // act
            pool.ToList().ForEach(c => Logger.LogInformation($"Macro: {c.Id}/{c.Name}-{c.Description}"));

            // assert
            Assert.That(pool.Any());
        }
    }
}
