using Bim.Application.IRCWood.Common;
using Bim.Application.IRCWood.IRC;
using Bim.Domain.Ifc;
using Bim.Domain.Polygon;
using IRCWoodWall.Physical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bim.Application.IRCWood.Physical
{
    public class WoodFrame
    {

        #region Properties


        public IfModel IfModel { get; set; }
<<<<<<< HEAD
<<<<<<< HEAD

=======
>>>>>>> parent of c0e791d... Merge branch 'BOQ' into Algorithm-MVC
=======
>>>>>>> parent of c0e791d... Merge branch 'BOQ' into Algorithm-MVC
        private List<WallFrame> WallFrames { get; set; }
        private List<FloorFrame> FloorFrames { get; set; }
        #endregion

        #region Collections
        private List<WallPolygon> WallPolygonCollection { get; set; }
        private List<FloorPolygon> FloorPolygonCollection { get; set; }
        private FloorPolygon Roof { get; set; }
        #endregion
        
        #region Tables
        public Table502_3_1 JoistTable { get; set; }
        public StudTable StudTable { get; set; }

        #endregion

<<<<<<< HEAD
<<<<<<< HEAD

        List<WallFrame> WallFrames { get; set; }
        List<FloorFrame> FloorFrames { get; set; }

=======
>>>>>>> parent of c0e791d... Merge branch 'BOQ' into Algorithm-MVC
=======
>>>>>>> parent of c0e791d... Merge branch 'BOQ' into Algorithm-MVC

        #region Constructor
        public WoodFrame()
        {

        }
        public WoodFrame(IfModel ifModel)
        {
            IfModel = ifModel;
            
        }
        #endregion

        #region Methods

        public void FrameWalls()
        {
<<<<<<< HEAD
<<<<<<< HEAD

            
            var studTable =  StudTable.Load(@"..\..\Tables\StudSpacingTable.txt");
            Table502_3_1 JoistTableLivingAreas = Table502_3_1.Load(@"..\..\..\IRCWoodWall\Tables\table502.3.1(2).csv");
            Table502_3_1 JoistTableSleepingAreas = Table502_3_1.Load(@"..\..\..\IRCWoodWall\Tables\table502.3.1(1).csv");
            Table502_5 HeadersTableExterior = Table502_5.Load(@"..\..\..\IRCWoodWall\Tables\table502.5(1).csv");
            Table502_5 HeadersTableInterior = Table502_5.Load(@"..\..\..\IRCWoodWall\Tables\table502.5(2).csv");


            var walls = IfModel.Instances.OfType<IfWall>().ToList();
            var polygons = WallPolygon.GetPolygons(walls);

=======
>>>>>>> parent of c0e791d... Merge branch 'BOQ' into Algorithm-MVC
=======
>>>>>>> parent of c0e791d... Merge branch 'BOQ' into Algorithm-MVC
            WallFrame wf;
            foreach (var polygon in WallPolygonCollection)
            {
                try
                {
                    wf = new WallFrame(polygon);
                    wf.StudTable = StudTable;
                    wf.New();
                }
                catch (Exception e)
                {

                    Console.WriteLine("Error: " + e.Message);
                }

            }
        }
        public void GetPolygons()
        {
            Task<List<WallPolygon>> GetWallPolygonsAsync = Task.Factory.StartNew(() =>
           {
               //get walls from wall collection
               return WallPolygon.GetPolygons(IfModel.WallCollection);
           });

            WallPolygonCollection = GetWallPolygonsAsync.Result;

            
        }
        public void Optimize()
        {
            Task OptimizeWalls = Task.Factory.StartNew(() =>
            {

                //Method implementation here
            });
            Task OptimizeFloor = Task.Factory.StartNew(() =>
            {

                //Method implementation here
            });
            Task OptimizeRoof = Task.Factory.StartNew(() =>
            {

                //Method implementation here
            });

        }
        public void Write()
        {
            #region WriteIfcAsync
            Task WriteIfcAsync = Task.Factory.StartNew(() =>
            {

                //Method implementation here
                FrameWalls();

            });

            WriteIfcAsync.Wait();
            #endregion

            #region WriteExcelAsync
            Task WriteExcelAsync = Task.Factory.StartNew(() =>
            {

                //Method implementation here
            });
            #endregion


        }
        #endregion

        #region Asyc Methods

       
        #endregion
    }
}


