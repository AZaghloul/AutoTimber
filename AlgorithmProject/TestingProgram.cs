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

namespace AlgorithmProject
{
    class TestingProgram
    {
        static string filepath = @"..\..\Models\ARCH-MODELv2.ifc";

        static void Main(string[] args)
        {
            #region Header
            //Header
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Starting....");
            Console.ResetColor();
            Console.WriteLine("-------------------------- ");
            #endregion

            List<Wall> walls = new List<Wall>();


            //open IfcModel
           var Ifcmodel= IfcHandler.Open(filepath);

            //new Model from IfcModel
            Model model = new Model();
            model.Initialize(Ifcmodel);
            //
            Wall.GetIfcWalls(model);
            walls= Wall.ExtractWalls();

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
