using Bim.Application.IRCWood.IRC;
using Bim.Domain.Ifc;
using Bim.Domain.Polygon;
using System;
using System.Collections.Generic;
using System.IO;
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

            var studTable = StudTable.Load(StudTable.FilePath);
            var exist = File.Exists(Table502_3_1.JoistTableLivingAreasPath);
            Table502_3_1 JoistTableLivingAreas = Table502_3_1.Load(Table502_3_1.JoistTableLivingAreasPath);
            Table502_3_1 JoistTableSleepingAreas = Table502_3_1.Load(Table502_3_1.JoistTableSleepingAreasPath);
            Table502_5 HeadersTableExterior = Table502_5.Load(Table502_5.HeadersTableExteriorPath);
            Table502_5 HeadersTableInterior = Table502_5.Load(Table502_5.HeadersTableInteriorPath);


            var walls = IfModel.Instances.OfType<IfWall2>().ToList();
            var polygons = WallPolygon2.GetPolygons(walls);
            WallFrame wf;
            for (int i = 0; i < polygons.Count; i++)
            {
                wf = new WallFrame(polygons[i]);
                wf.StudTable = studTable;
                wf.HeadersTable = HeadersTableExterior;
                wf.New2();
            }
            //foreach (var polygon in polygons)
            //{
            //    try
            //    {
            //        wf = new WallFrame(polygon);
            //        wf.StudTable = studTable;
            //        wf.New2();
            //    }
            //    catch (Exception e)
            //    {

            //        Console.WriteLine("Error: " + e.Message);
            //    }


            //}


        }




        public void FrameFloors()
        {
            var studTable = StudTable.Load(StudTable.FilePath);
            var exist = File.Exists(Table502_3_1.JoistTableLivingAreasPath);
            Table502_3_1 JoistTableLivingAreas = Table502_3_1.Load(Table502_3_1.JoistTableLivingAreasPath);
            Table502_3_1 JoistTableSleepingAreas = Table502_3_1.Load(Table502_3_1.JoistTableSleepingAreasPath);
            Table502_5 HeadersTableExterior = Table502_5.Load(Table502_5.HeadersTableExteriorPath);
            Table502_5 HeadersTableInterior = Table502_5.Load(Table502_5.HeadersTableInteriorPath);

            var Floors = IfModel.Instances.OfType<IfFloor>().ToList();
            var floorPolygon = FloorPolygon.GetPolygons(Floors);
            FloorFrame.FloorSetup();
            FloorFrame Ff;
            foreach (var polygon in floorPolygon)
            {
                try
                {
                    Ff = new FloorFrame(polygon);
                    Ff.JoistTable = JoistTableLivingAreas;
                    Ff.New();
                }
                catch (Exception e)
                {

                    Console.WriteLine("Error: " + e.Message);
                }


            }
            #endregion
        }
    }
}


