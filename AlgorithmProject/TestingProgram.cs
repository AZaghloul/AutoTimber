using System.Collections.Generic;
using System.Linq;
using System;
using Bim.IO.Utilities;
using Bim.Domain.Ifc;
using Xbim.Ifc4.SharedBldgElements;
using Bim.Domain.Polygon;
using Bim.Application.IRCWood.Physical;
using Bim.Domain;
using Bim.Application.IRCWood.Common;
using Bim.Application.IRCWood.IRC;
using System.Diagnostics;
using Bim.IO;
using System.IO;

namespace AlgorithmProject
{
    class TestingProgram
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger
     (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        static void Main(string[] args)
        {

            #region Header
            "Wall Framing Solutions ".Header(ConsoleColor.Yellow, ConsoleColor.Black);
            "Starting....".Print(ConsoleColor.Cyan);
            "".PrintAtPosition(x: 10, foreColor: ConsoleColor.Red);
            "-------------------------------------------- ".Print(ConsoleColor.White);

            #endregion
            StudTable.FilePath = @"..\..\Tables\StudSpacingTable.csv";
            Table502_3_1.FilePath = @"..\..\Tables\table502.3.1(1).txt";
            string fileName = @"..\..\Models\ITI.Qondos.2-Solved.ifc";
            string outPut = Path.GetFileNameWithoutExtension(fileName)+ "-structure"+".ifc";
            //   IfModel model = IfModel.Open(fileName);

            


            Stopwatch sw = new Stopwatch();
            sw.Start();


            var startup = new IfStartup();
            IfModel model = IfModel.Open(fileName);
            model.Delete<IfcWall>();
            model.Save(outPut);
            IfcHandler.ToWexBim(outPut);
            //
            WoodFrame wf = new WoodFrame(model);
            startup.Configure(model,wf);
            startup.Configuration(model);
            
            wf.GetPolygons();
            wf.Optimize();
            wf.Write();
            model.Save(outPut);
            model.Delete<IfcBeam>();
            model.Delete<IfcColumn>();
            sw.Stop();
            Console.WriteLine($"time Elapsed{sw.ElapsedMilliseconds}");
            OpenWindow(fileName + "-structure"+".ifc");
            OpenWindow(outPut);

            List<IfWall> walls = model.WallCollection.OfType<IfWall>().ToList();
            $"{walls.Count} walls are found".Print(ConsoleColor.Cyan);
            List<WallPolygon> wallPolygons = new List<WallPolygon>();
            int i = 0;
            foreach (var item in walls)
            {
                $"wall no {i}".Print(ConsoleColor.Cyan);
                wallPolygons.Add(new WallPolygon(item));
                $"{wallPolygons.Last().Regions.Count} regions are found".Print(ConsoleColor.Cyan);
                $"\t {wallPolygons.Last().Openings.Count} opens".Print(ConsoleColor.Cyan);
                $"\t {wallPolygons.Last().RLeft.Count} left regions".Print(ConsoleColor.Cyan);
                $"\t {wallPolygons.Last().RRight.Count} Right regions".Print(ConsoleColor.Cyan);
                $"\t {wallPolygons.Last().RTop.Count} top regions".Print(ConsoleColor.Cyan);
                $"\t {wallPolygons.Last().RBottom.Count} bot regions".Print(ConsoleColor.Cyan);
                $"\t {wallPolygons.Last().RBetween.Count} middle regions".Print(ConsoleColor.Cyan);
                i++;
            }

            #region Footer
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Done!");
            #endregion
            Console.ReadLine();

        }
        public static void OpenWindow(string filePath)
        {
            System.Diagnostics.Process.Start(filePath);
        }
      


    }


}
