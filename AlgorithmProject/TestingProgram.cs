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
using Bim.Components;
using Bim.Utilities;

namespace AlgorithmProject
{
    class TestingProgram
    {
        
        
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger
     (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        static string fileopen = @"..\..\Models\walls-noColumn.ifc";
        static string newFile = @"..\..\Models\Beam-stud.ifc";

        static void Main(string[] args)
        {
            "enter x:".GetInput(foreColor: ConsoleColor.Red);
             "Wall Framing Long Header ".Header(ConsoleColor.Yellow,ConsoleColor.Black);
            "iam in the center".PrintCenter(ConsoleColor.Cyan);
            "testing Position Function".PrintAtPosition(x:10, foreColor: ConsoleColor.Red);


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
            walls= Wall.ExtractWalls(model);
            using (var txn=model.IfcModel.BeginTransaction("DeletingColumn"))
            {
                var columns = model.IfcModel.Instances.OfType<IIfcColumnStandardCase>();
                int c = columns.Count();
                for (int i = 0; i < c; i++)
                {

                    model.IfcModel.Delete(columns.FirstOrDefault());


                }

                txn.Commit();
                model.IfcModel.SaveAs(@"..\..\Models\walls-noColumn.ifc");
            }
             int count = walls.Count();
            for (int i = 0; i < count; i++)
            {


                Stud mystud = new Stud(beamModel, walls[i], new Location(0, 0, 0), new Dimension(1, 1, 8), "New-Stud");
                // Sill mysill = new Sill(beamModel, new Location(3, -8, 0), new Dimension(6, 1, 1), "New-Sill");
                Stud mystud2 = new Stud(beamModel, walls[i], new Location(2, 0, 0), new Dimension(1, 1, 8), "New-Stud");
                Stud mystud3 = new Stud(beamModel, walls[i], new Location(4, 0, 0), new Dimension(1, 1, 8), "New-Stud");
            }
           // beamModel.Save(newFile,true);
            model.Save(fileopen , true);
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
