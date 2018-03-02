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
using BIMSpace.General;
using System;
using BIMSpace.Extensions;
namespace AlgorithmProject
{
    class Program
    {
        static string filepath = @"..\..\Models\ARCH-MODELv2.ifc";

        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Starting....");

            Wall.GetWalls(filepath);
            var myWalls = Wall.ExtractWalls();
            myWalls[0].CreateWall();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Done...");

            Console.ReadLine();
        }
    }
}
