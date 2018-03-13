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

using Bim.Common;
using Bim.Extensions;
using log4net;
using BIMSpace.Components;

namespace AlgorithmProject
{
    class TestingProgram
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger
     (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        static string fileopen = @"..\..\Models\ThreeWalls.ifc";
        static string newFile = @"..\..\Models\Beam-new.ifc";

        static void Main(string[] args)
        {
            #region Header
            //Header
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Starting....");
            Console.ResetColor();
            Console.WriteLine("-------------------------- ");
            #endregion
         //   log.Debug("Starting Logging");

            List<Wall> walls = new List<Wall>();


            //open IfcModel

            var model=  Model.Open(fileopen);
            var beamModel = Model.NewModel("Stud","ITI-Building" ,true, newFile);
              Wall.GetIfcWalls(model);
             walls= Wall.ExtractWalls();

            Stud mystud = new Stud(beamModel, new Location(0,0,0),new Dimension(300,300,8000),"New-Stud");
            Stud mystud2 = new Stud(beamModel, new Location(0, 0, 3000), new Dimension(400, 400, 6000), "New-Stud2");

           
            beamModel.Save(newFile,true);
           // model.OpenWindow(filepath);
            #region Footer

            Console.WriteLine($"{walls.Count} wall(s) found!");
            //Footer
            Console.WriteLine("-------------------------- ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Done!");
            #endregion
            Console.ReadLine();
        }
    }
}
