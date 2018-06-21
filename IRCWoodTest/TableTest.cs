using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Bim.Application.IRCWood.IRC;
using System.Collections.Generic;
using System.Linq;

namespace Bim.Application.IRCWood.IRC
{
    [TestClass]
    public class TableTest
    {
        [TestMethod]
        public void LoadTest()
        {
            //Table502_3_1 table = GetTable(1);
            Table502_5 table2 = Table502_5.Load(@"F:\ITI Projects\ITI.Qondos\AlgorithmProject\IRCWoodWall\Tables\table502.5(2).csv");
            Assert.IsTrue(true);
        }

        private static Table502_3_1 GetTable(int x)
        {
            switch (x)
            {
                case 1:
                    return Table502_3_1.Load(@"F:\ITI Projects\ITI.Qondos\AlgorithmProject\IRCWoodWall\Tables\table502.3.1(1).csv");
                case 2:
                    return Table502_3_1.Load(@"F:\ITI Projects\ITI.Qondos\AlgorithmProject\IRCWoodWall\Tables\table502.3.1(2).csv");
                default:
                    return Table502_3_1.Load(@"F:\ITI Projects\ITI.Qondos\AlgorithmProject\IRCWoodWall\Tables\table502.3.1(2).csv");
            }
        }

        [TestMethod]
        public void GetCellsTest()
        {
            Table502_3_1 table1 = GetTable(1);
            Table502_3_1 table2 = GetTable(2);

            List<TableCell502_3_1> T1cells = table1.Cells.Where(
                e => 
                e.WoodType == WoodType.Douglas_fir_larch &&
                e.WoodGrade == WoodGrade.SS &&
                e.Span.Inches >= 15 * 12 &&
                e.Section.Depth.Inches == 8).OrderBy(e=> e.Span.Inches).ToList();
            TableCell502_3_1 tableCell = T1cells.FirstOrDefault();
            List<TableCell502_3_1> T2cells = table2.GetCells(WoodType.Southern_pine, WoodGrade._1, 10, 20 * 12,7);
            TableCell502_3_1 tableCell1 = T2cells.FirstOrDefault();
            Assert.IsTrue(true);

        }
    }
}
