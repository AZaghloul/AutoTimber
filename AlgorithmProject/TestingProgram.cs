using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xbim.Ifc;
using Xbim.Ifc.ViewModels;
using Xbim.Ifc4.GeometricConstraintResource;
using Xbim.Ifc4.Interfaces;
using Xbim.Ifc4.MeasureResource;
using Xbim.Common;
using System;
using log4net;
using Bim.Application.Ifc;
using Bim.IO.Utilities;
using Bim.Domain.Ifc;
using Xbim.Ifc4.SharedBldgElements;
using System.IO.Ports;
using Bim.Domain;
using Bim.Domain.Polygon;

namespace AlgorithmProject
{
    class TestingProgram
    {


        private static readonly log4net.ILog log = log4net.LogManager.GetLogger
     (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);



        static void Main(string[] args)
        {
            //Start();
            #region Header

            "Wall Framing Solutions ".Header(ConsoleColor.Yellow, ConsoleColor.Black);
            "Starting....".Print(ConsoleColor.Cyan);
            "".PrintAtPosition(x: 10, foreColor: ConsoleColor.Red);
            "-------------------------------------------- ".Print(ConsoleColor.White);
            #endregion

            string fileName = @"..\..\Models\P1.ifc";
            IfModel model = IfModel.Open(fileName);
            model.Delete<IfcBeamStandardCase>();
            model.Delete<IfcColumnStandardCase>();
            List<IfWall> walls = IfWall.ExtractWalls(model);

            $"{walls.Count} walls are found".Print(ConsoleColor.Cyan);

            List<WallPolygon> wallPolygons = new List<WallPolygon>();
            int i = 0;
            foreach (var item in walls)
            {
                $"wall no {i}".Print(ConsoleColor.Cyan);
                wallPolygons.Add(new WallPolygon(item));
                $"{wallPolygons.Last().Regions.Count} regions are found".Print(ConsoleColor.Cyan);
                $"\t {wallPolygons.Last().Opens.Count} opens".Print(ConsoleColor.Cyan);
                $"\t {wallPolygons.Last().RLeft.Count} left regions".Print(ConsoleColor.Cyan);
                $"\t {wallPolygons.Last().RRight.Count} Right regions".Print(ConsoleColor.Cyan);
                $"\t {wallPolygons.Last().RTop.Count} top regions".Print(ConsoleColor.Cyan);
                $"\t {wallPolygons.Last().RBottom.Count} bot regions".Print(ConsoleColor.Cyan);
                $"\t {wallPolygons.Last().RBetween.Count} middle regions".Print(ConsoleColor.Cyan);
                i++;
            }


            model.Save(fileName);

            OpenWindow(fileName);





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
            
            var beamModel = IfModel.NewModel("Stud", "ITI-Building", true, newFile);
            IfWall.GetIfcWalls(model);
            walls = IfWall.ExtractWalls(model);

            Console.WriteLine($"{walls.Count} wall(s) found!");
            //Footer
            Console.WriteLine("-------------------------- ");


            int count = walls.Count();

            for (int i = 0; i < count; i++)
            {
                IfStud mystud = new IfStud(beamModel, walls[i], new IfLocation(0, 0, 0), new IfDimension(.6f, .3f, 4), "New-Stud");
                // IfcSill mysill = new IfcSill(beamModel, new IfLocation(3, -8, 0), new IfDimension(6, 1, 1), "New-Sill");
                IfStud mystud2 = new IfStud(beamModel, walls[i], new IfLocation(2, 0, 0), new IfDimension(.6f, .3f, 4), "New-Stud");
                IfStud mystud3 = new IfStud(beamModel, walls[i], new IfLocation(4, 0, 0), new IfDimension(.6f, .3f, 4), "New-Stud");
            }
            beamModel.Save(newFile);
            model.Save(fileopen);
            OpenWindow(fileopen);
        }
        public static void Start()
        {
            SerialPort serial = new SerialPort("COM4", 9600, 0, 6, StopBits.One);
            //serial.Handshake = Handshake.RequestToSend;
           // serial.
            serial.Open();
          var strr=  serial.ReadExisting();
            var str = SerialPort.GetPortNames();
           
        }
    }


}
