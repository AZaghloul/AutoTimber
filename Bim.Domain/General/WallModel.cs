using Bim.Common.Geometery;
using Bim.Domain.Ifc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bim.Domain.General
{
    public class WallModel
    {
        public IfWall IfWall { get; set; }
        public IfLocation IfLocation { get; set; }
        public IfDimension IfDimension { get; set; }
        public Direction Direction { get; set; }
        public IfLocation MidPoint { get; set; }
        public IfLocation EndPoint { get; set; }
        public WallModel(IfWall ifWall)
        {
            IfWall = ifWall;
            IfLocation = new IfLocation(IfWall.IfLocation);
            IfDimension = new IfDimension(IfWall.IfDimension);
            Direction = IfWall.Direction;
            if (Direction == Direction.Negative)
            {
                Flip(Axis.Other);
                Direction = Direction.Positive;

            }
            MidPoint = Vector3D.DivideDistance(IfWall.IfLocation, IfWall.IfDimension);
            EndPoint = GetEndPoint();
        }

        public void Flip(Axis axis)
        {
            switch (axis)
            {
                case Axis.xAxis:
                    IfLocation.X = IfLocation.X - IfDimension.XDim;
                    break;
                case Axis.yAxis:
                    IfLocation.Y = IfLocation.Y - IfDimension.YDim;
                    break;
                case Axis.zAxis:
                    IfLocation.Z = IfLocation.Z - IfDimension.ZDim;
                    break;
                default:
                    IfLocation.X = IfLocation.X - IfDimension.XDim;
                    IfLocation.Y = IfLocation.Y - IfDimension.YDim;
                    IfLocation.Z = IfLocation.Z - IfDimension.ZDim;
                    break;
                    
            }
        }
        private IfLocation GetEndPoint()
        {
             return new IfLocation(IfLocation.X + IfDimension.XDim,
                IfLocation.Y + IfDimension.YDim,
                IfLocation.Z + IfDimension.ZDim);

            
           
        }
    }
}
