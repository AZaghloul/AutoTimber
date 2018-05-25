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
            var dimensions = new Dictionary<string, IfDimension>();
            dimensions.Add("Stud", new IfDimension(2, 6, 0));
            dimensions.Add("TopPlate", new IfDimension(0, .6, .2));
            dimensions.Add("BottomPlate", new IfDimension(0, .6, .2));
            dimensions.Add("Header", new IfDimension(0, .6, .6));
            var studTable =  StudTable.Load(@"..\..\Tables\StudSpacingTable.txt");

            var walls = IfModel.Instances.OfType<IfWall>().ToList();
            var polygons = WallPolygon.GetPolygons(walls);
            WallFrame wf;
            foreach (var polygon in polygons)
            {
                try
                {
                    wf = new WallFrame(polygon);
                    wf.Dimensions = dimensions;
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


