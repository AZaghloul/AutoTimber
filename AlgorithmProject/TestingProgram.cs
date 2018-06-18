using System.Collections.Generic;
using System.Linq;
using System;
using Bim.IO.Utilities;
using Bim.Domain.Ifc;
using Xbim.Ifc4.SharedBldgElements;
using Bim.Domain.Polygon;
using Bim.Domain.Configuration;
using Bim.Common.Measures;
using Bim.Application.IRCWood.Physical;
using Bim.Common.Geometery;
using Bim.Domain;
using Bim.Application.IRCWood.IRC;
using Xbim.Ifc;

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
            var d = Split.Equal(13, .65);

            string fileName = @"..\..\Models\ITI.Qondos.1FloorOnly.ifc";
            string saveName = fileName.Split(new string[] { ".ifc" }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault() + @"-Solved.ifc";

            IfcStore ifcStore = IfcStore.Open(fileName);
            ifcStore.SaveAs(fileName, Xbim.IO.IfcStorageType.IfcXml);

            using (IfModel model = IfModel.Open(fileName))
            {
                Startup.Configuration(model);
                model.Save(fileName);
                //model.Delete<IfcBeam>();
               // model.Delete<IfcColumn>();
                WoodFrame wf = new WoodFrame(model);
                wf.FrameWalls();
                //model.Delete<IfcWall>();
                model.Save(saveName);
               // OpenWindow(fileName);
                OpenWindow(saveName);

                List<IfWall> walls = model.Instances.OfType<IfWall>().ToList();
                List<IfFloor> floors = model.Instances.OfType<IfFloor>().ToList();

                $"{walls.Count} walls are found".Print(ConsoleColor.Cyan);
                $"{floors.Count} floors are found".Print(ConsoleColor.Cyan);

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
