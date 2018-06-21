using System.Collections.Generic;
using System.Linq;
using System;
using Bim.IO.Utilities;
using Bim.Domain.Ifc;
using Xbim.Ifc4.SharedBldgElements;
using Bim.Domain.Polygon;

using Bim.Application.IRCWood.Physical;
using Bim.Common.Geometery;
using Bim.Domain;
using Bim.Application.IRCWood.IRC;
using Xbim.Ifc;
using Bim.BOQ;
using Xbim.Ifc4.Interfaces;
using Xbim.Ifc4.ProductExtension;
using Bim.Domain.Ifc.Enums;
using Bim.Domain.Configuration;

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

            string fileName = @"..\..\Models\ITI.Qondos.2.ifc";
            string saveName = fileName.Split(new string[] { ".ifc" }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault() + @"-Solved.ifc";

            IfcStore ifcStore = IfcStore.Open(fileName);
            //  ifcStore.SaveAs(fileName, Xbim.IO.IfcStorageType.IfcXml);
            var beams = ifcStore.Instances.OfType<IIfcBeam>();

            var prop = new IfProperties((IfcBuildingElement)beams.FirstOrDefault());
            var sList = new List<IfSingleValue>()
            {
                new IfSingleValue("test value1", "100"),
                new IfSingleValue("test value2", "400"),
                new IfSingleValue("test value2", "300"),
             };
            var qList = new List<IfQuantity>()
            {
                new IfQuantity("test Quantity  value1", "100",IfUnitEnum.AREAUNIT),
                new IfQuantity("test Quantity value2", "200",IfUnitEnum.AREAUNIT),
                new IfQuantity("test Quantity value2", "300",IfUnitEnum.AREAUNIT),
            };
            //prop.AddSingleValue("sss value List", sList);
            //prop.AddQuantities("Quantity value List", qList);
            //prop.FindByName("Join Status");
            //prop.FindByValue("Both joins enabled");
            //prop.FindSVProperty(new IfSingleValue("Join Status", "Both joins enabled"));
        //prop.New();
        //    ifcStore.SaveAs((fileName + "prop"));

            using (IfModel model = IfModel.Open(fileName))
            {
                Startup.Configuration(model);
                model.Save(fileName);
                model.Delete<IfcBeam>();
                model.Delete<IfcColumn>();
                WoodFrame wf = new WoodFrame(model);
                wf.FrameWalls();
                model.Delete<IfcWall>();
                model.Delete<IfcSlab>();

                model.Save(saveName);
                // OpenWindow(fileName);
              //  OpenWindow(saveName);

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
                GeometryCollection GC1 = new GeometryCollection();
                GC1.AddToCollection(model.Instances.OfType<IfJoist>());
                GC1.AddToCollection(model.Instances.OfType<IfStud>());
                GC1.AddToCollection(model.Instances.OfType<IfSill>());
               var coll= GC1.ToExcel(GC1.BOQTable, "Testing Excel", false,"Number", "Collection");
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
