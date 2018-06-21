using Bim.Application.IRCWood.IRC;
using Bim.Domain.Ifc;
using Bim.Domain.Polygon;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bim.Application.IRCWood.Physical
{
    public class WoodFrame
    {

        public IfModel IfModel { get; set; }
        List<WallFrame> WallFrames { get; set; }
        List<FloorFrame> FloorFrames { get; set; }

        #region Constructor
        public WoodFrame(IfModel ifModel)
        {
            IfModel = ifModel;
        }
        public WoodFrame()
        {

        }
        #endregion
        #region Methods
        public void FrameWalls()
        {
            
            var studTable =  StudTable.Load(@"..\..\Tables\StudSpacingTable.txt");
            Table502_3_1 JoistTableLivingAreas = Table502_3_1.Load(@"..\..\..\IRCWoodWall\Tables\table502.3.1(2).csv");
            Table502_3_1 JoistTableSleepingAreas = Table502_3_1.Load(@"..\..\..\IRCWoodWall\Tables\table502.3.1(1).csv");
            Table502_5 HeadersTableExterior = Table502_5.Load(@"..\..\..\IRCWoodWall\Tables\table502.5(1).csv");
            Table502_5 HeadersTableInterior = Table502_5.Load(@"..\..\..\IRCWoodWall\Tables\table502.5(2).csv");


            var walls = IfModel.Instances.OfType<IfWall>().ToList();
            var polygons = WallPolygon.GetPolygons(walls);
            WallFrame wf;
            foreach (var polygon in polygons)
            {
                try
                {
                    wf = new WallFrame(polygon);
                    wf.StudTable = studTable;
                    wf.New();
                }
                catch (Exception e)
                {

                    Console.WriteLine("Error: " + e.Message);
                }


            }

            var Floors = IfModel.Instances.OfType<IfFloor>().ToList();
            var floorPolygon = FloorPolygon.GetPolygons(Floors);
            FloorFrame Ff;
            foreach (var polygon in floorPolygon)
            {
                try
                {
                    Ff = new FloorFrame(polygon);
                    Ff.JoistTable= JoistTableLivingAreas;
                    Ff.New();
                }
                catch (Exception e)
                {

                    Console.WriteLine("Error: " + e.Message);
                }


            }


        }
        #endregion
    }
}


