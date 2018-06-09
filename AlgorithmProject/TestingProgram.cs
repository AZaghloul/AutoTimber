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
          var d=  Split.Equal(13, .65);
            
            string fileName = @"..\..\Models\ITI.Qondos.ifc";
            string saveName = fileName.Split(new string[] { ".ifc" }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault() + @"-Solved.ifc";

            Table502_3_1 tableSleepingArea = Table502_3_1.Load(@"F:\ITI Projects\ITI.Qondos\AlgorithmProject\IRCWoodWall\Tables\table502.3.1(1).csv");
            Table502_3_1 tableLivingArea = Table502_3_1.Load(@"F:\ITI Projects\ITI.Qondos\AlgorithmProject\IRCWoodWall\Tables\table502.3.1(2).csv");

            using (IfModel model = IfModel.Open(fileName))
            {
                Startup.Configuration(model);
                model.Delete<IfcBeam>();
                model.Delete<IfcColumn>();
                var doors = model.Instances.OfType<IfOpening>().Where(e => e.OpeningType == OpeningType.Door).ToList();
                WoodFrame wf = new WoodFrame(model);
                wf.FrameWalls();
                //model.Delete<IfcWall>();
                model.Save(saveName);
                OpenWindow(fileName);
                OpenWindow(saveName);

                List<IfWall> walls = model.Instances.OfType<IfWall>().ToList();
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
        public static void CreateStuds()
        {
            string fileopen = @"..\..\Models\walls-noColumn.ifc";
            string newFile = @"..\..\Models\Beam-stud.ifc";
            List<IfWall> walls = new List<IfWall>();
            //open IfcModel
            var model = IfModel.Open(fileopen);

            var beamModel = IfModel.New("Stud", "ITI-Building", true, newFile);
            //IfWall.GetIfcWalls(model);
            //walls = IfWall.ExtractWalls(model);

            Console.WriteLine($"{walls.Count} wall(s) found!");
            //Footer
            Console.WriteLine("-------------------------- ");


            int count = walls.Count();

            //for (int i = 0; i < count; i++)
            //{
            //    IfStud mystud = new IfStud(beamModel, walls[i], new IfLocation(0, 0, 0), new IfDimension(.6f, .3f, 4), "New-Stud");
            //    // IfcSill mysill = new IfcSill(beamModel, new IfLocation(3, -8, 0), new IfDimension(6, 1, 1), "New-Sill");
            //    IfStud mystud2 = new IfStud(beamModel, walls[i], new IfLocation(2, 0, 0), new IfDimension(.6f, .3f, 4), "New-Stud");
            //    IfStud mystud3 = new IfStud(beamModel, walls[i], new IfLocation(4, 0, 0), new IfDimension(.6f, .3f, 4), "New-Stud");
            //}
            beamModel.Save(newFile);
            model.Save(fileopen);
            OpenWindow(fileopen);
        }


    }


}
