using Bim.Application.IRCWood.IRC;
using Bim.Domain.Ifc;
using Bim.Domain.Polygon;
using IRCWoodWall.Physical;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bim.Application.IRCWood.Physical
{
    public class WoodFrame
    {

        public IfModel IfModel { get; set; }
        List<WallFrame> WallFrames { get; set; }
        public List<FloorFrame> FloorFrames { get; set; }

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
            
            var studTable =  StudTable.Load();
            Table502_3_1 JoistTable = Table502_3_1.Load(null);

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


        }
        #endregion
    }
}


