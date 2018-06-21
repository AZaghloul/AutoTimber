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


