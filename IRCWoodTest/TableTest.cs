using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Bim.Application.IRCWood.IRC;
namespace Bim.Application.IRCWood.IRC
{
    [TestClass]
    public class TableTest
    {
        [TestMethod]
        public void LoadTest()
        {
            Table502_3_1 table = Table502_3_1.Load(@"F:\ITI Projects\ITI.Qondos\AlgorithmProject\IRCWoodWall\Tables\table502.3.1(1).txt");

            Assert.IsTrue(true);
        }
    }
}
