using System.Collections.Generic;
using System.Linq;
using Xbim.Ifc;
using Xbim.Ifc4.Interfaces;
using System;
using Xbim.Common;
using Xbim.Common.Metadata;
using Xbim.Ifc4.GeometryResource;
using Xbim.Ifc4.MeasureResource;

namespace Bim.Domain.Ifc
{
    public class IfDirection
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public IfDirection(IIfcCartesianPoint P1, IIfcCartesianPoint P2)
        {
            X = P2.X - P1.X;
            Y = P2.Y - P1.Y;
            if (!double.IsNaN(P1.Z) && !double.IsNaN(P2.Z))
                Z = P2.Z - P1.Z;
            else Z = 0;

            Normalize();
        }

        private void Normalize()
        {
            double Length = Math.Sqrt(X * X + Y * Y + Z * Z);
            X = X / Length;
            Y = Y / Length;
            Z = Z / Length;
        }
    }
}