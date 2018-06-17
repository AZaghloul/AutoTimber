using System;
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
using Bim.Domain.Ifc;

namespace Bim.Domain.Polygon
{
    public class FloorPolygon
    {
        #region Properties
        public List<Region> Regions { get; set; }
        public IfFloor IfFloor { get; set; }
        #endregion

        public FloorPolygon(IfFloor ifFloor)
        {
            IfFloor = ifFloor;
            Regions = new List<Region>();
            GetRegions();
        }


        #region Methods
        public void GetRegions()
        {
            var reg = new Region()
            {
                IfDimension = new IfDimension(IfFloor.IfDimension),
                IfLocation = new IfLocation(),
                RegionLocation = RegionLocation.Left,
            };
            Regions.Add(reg);
        }
        #endregion

        #region Static Methods
        public static List<FloorPolygon> GetPolygons(List<IfFloor> Floors)
        {
            List<FloorPolygon> res = new List<FloorPolygon>();
            foreach (var Floor in Floors)
            {
                res.Add(new FloorPolygon(Floor));
            }
            return res;
        }
        #endregion

    }
}