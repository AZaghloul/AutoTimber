using System.Collections.Generic;
using System.Linq;
using Xbim.Ifc;
using Xbim.Ifc4.Interfaces;
using System;
using Bim.Domain.Configuration;
using MathNet.Spatial.Euclidean;

namespace Bim.Domain.Ifc
{
    public class IfFloor : IfElement
    {
        public static Setup Setup { get; set; }
        #region Floor Object Properties

        public IfStory Story { get; set; }
        public IIfcSlab IfcSlab { get; set; }
        public IIfcPolyline PolyLine { get; set; }
        public IIfcAxis2Placement3D SlabAxis { get; set; }
        public IIfcLocalPlacement LocalPlacement { get; set; }
        public IfDirection ShortDirection { get; set; }

        public IfDirection LongDirection { get; set; }
        #endregion

        #region Constructors

        public IfFloor()
        {

        }
        public IfFloor(IfModel ifModel, IIfcSlab ifcSlab)
        {
            IfcSlab = ifcSlab;
            IfModel = ifModel;
            IfModel.Instances.Add(this);
            Initialize();
        }
        #endregion

        private void Initialize()
        {
            Guid = IfcSlab.GlobalId;
            Name = IfcSlab.Name;
            Label = IfcSlab.EntityLabel;
            PolyLine = IfcSlab.Representation.Representations.SelectMany(a => a.Items).
                OfType<IIfcPolyline>().FirstOrDefault();
            GetLocation();
            GetDimension();
        }
        private void GetLocation()
        {
            var Loc = PolyLine.Points[0];
            SlabAxis = ((IIfcAxis2Placement3D)((IIfcLocalPlacement)IfcSlab.ObjectPlacement).RelativePlacement);
            var location = SlabAxis.Location;
            LocalPlacement = (IIfcLocalPlacement)IfcSlab.ObjectPlacement;
            IfLocation = new IfLocation((float)Loc.X, (float)Loc.Y, (float)Loc.Z);
        }
        private void GetDimension()
        {
            //get the wall x,y,z

            double shortDimension = Math.Sqrt(GetLengthSquare(PolyLine.Points[0], PolyLine.Points[1]));
            //
            ShortDirection = new IfDirection(PolyLine.Points[0], PolyLine.Points[1]);
           
            double LongDimension = 0;

            int noOfPoints = PolyLine.Points.ToArray().Length;

            double GetLengthSquare(IIfcCartesianPoint P1, IIfcCartesianPoint P2)
            {
                if (!double.IsNaN(P1.Z) && !double.IsNaN(P2.Z))
                    return (Math.Pow(P1.X - P2.X, 2) + Math.Pow(P1.Y - P2.Y, 2) + Math.Pow(P1.Z - P2.Z, 2));
                else return (Math.Pow(P1.X - P2.X, 2) + Math.Pow(P1.Y - P2.Y, 2));

            }
            for (int i = 0; i < noOfPoints - 1; i++)
            {
                double Length = Math.Sqrt(GetLengthSquare(PolyLine.Points[i], PolyLine.Points[i + 1]));
                if (shortDimension > Length)
                {
                    shortDimension = Length;
                    ShortDirection = new IfDirection(PolyLine.Points[i], PolyLine.Points[i + 1]);
                }
                LongDimension = Math.Max(LongDimension, Length);
            }

            var depth =
                 IfcSlab.Representation.Representations
                        .SelectMany(a => a.Items)
                        .OfType<IIfcExtrudedAreaSolid>().Select(a => a.Depth).FirstOrDefault();


            if (depth == 0)
            {
                try
                {
                    depth = ((IIfcExtrudedAreaSolid)IfcSlab.Representation.Representations
                 .SelectMany(a => a.Items)
                 .OfType<IIfcBooleanClippingResult>().
                 Select(a => a.FirstOperand).FirstOrDefault()).Depth;
                }
                catch (System.Exception)
                {


                }
            }
            var sDir = new Vector3D
               (
               ShortDirection.X,
               ShortDirection.Y,
               ShortDirection.Z
               );
            var zDir = new Vector3D(0, 0, 1);
            var vec = zDir.CrossProduct(sDir);
            LongDirection = new IfDirection(vec.X, vec.Y, vec.Z);

            IfDimension = new IfDimension(shortDimension, LongDimension, depth);



            //var recD = wall.Representation.Representations.SelectMany(a => a.Items).OfType<IIfcExtrudedAreaSolid>().Select(a => a.SweptArea).OfType<IIfcRectangleProfileDef>().FirstOrDefault();
            //var recD1 = wall.Representation.Representations.SelectMany(a => a.Items).OfType<IIfcBooleanClippingResult>().Select(a => a.FirstOperand).OfType<IIfcExtrudedAreaSolid>().Select(a => a.SweptArea).OfType<IIfcRectangleProfileDef>().FirstOrDefault();
            //var otherDepth = wall.Representation.Representations.SelectMany(a => a.Items).OfType<IIfcBooleanClippingResult>().Select(a => a.FirstOperand).OfType<IIfcExtrudedAreaSolid>().Select(a => a.Depth);
            //var depth = wall.Representation.Representations.SelectMany(a => a.Items).OfType<IIfcExtrudedAreaSolid>().Select(a => a.Depth);
            //get the wall thickness
            //var thickness = IfcSlab.HasAssociations.OfType<IIfcRelAssociatesMaterial>().OfType<IIfcMaterialLayerSetUsage>().Select(a => a.OffsetFromReferenceLine);//.OfType<IfcPositiveLengthMeasure>();
            //var location = ((IIfcAxis2Placement3D)((IIfcLocalPlacement)IfcSlab.ObjectPlacement).RelativePlacement).Location;
            //using the Wall Class;

        }
        public static List<IfFloor> GetFloors(IfStory ifStory)
        {
            List<IfFloor> FloorList = new List<IfFloor>();
            var Floors = ifStory.IfcStory.ContainsElements
                 .FirstOrDefault()
                 .RelatedElements.OfType<IIfcSlab>();


            foreach (var Floor in Floors)
            {
                var dir = ((IIfcAxis2Placement3D)((IIfcLocalPlacement)Floor.ObjectPlacement).RelativePlacement).RefDirection;
                var recD = Floor.Representation.Representations
                                .SelectMany(a => a.Items)
                                .OfType<IIfcExtrudedAreaSolid>().Select(a => a.SweptArea)
                                .OfType<IIfcRectangleProfileDef>().FirstOrDefault() ??
                            Floor.Representation.Representations
                                .SelectMany(a => a.Items)
                                .OfType<IIfcBooleanClippingResult>().Select(a => a.FirstOperand)
                                .OfType<IIfcExtrudedAreaSolid>().Select(a => a.SweptArea)
                                .OfType<IIfcRectangleProfileDef>().FirstOrDefault() ??
                            Floor.Representation.Representations
                                .SelectMany(a => a.Items)
                                .OfType<IIfcBooleanClippingResult>().Select(a => a.FirstOperand)
                                .OfType<IIfcBooleanClippingResult>().Select(a => a.FirstOperand)
                                .OfType<IIfcExtrudedAreaSolid>().Select(a => a.SweptArea)
                                .OfType<IIfcRectangleProfileDef>().FirstOrDefault() ??
                            Floor.Representation.Representations
                                .SelectMany(a => a.Items)
                                .OfType<IIfcBooleanClippingResult>().Select(a => a.FirstOperand)
                                .OfType<IIfcBooleanClippingResult>().Select(a => a.FirstOperand)
                                .OfType<IIfcBooleanClippingResult>().Select(a => a.FirstOperand)
                                .OfType<IIfcExtrudedAreaSolid>().Select(a => a.SweptArea)
                                .OfType<IIfcRectangleProfileDef>().FirstOrDefault();


                //get the wall x,y,z
                if (recD != null)
                {
                    IfFloor crntFloor = new IfFloor(ifStory.IfModel, Floor)
                    {
                        Story = ifStory,
                        IfModel = ifStory.IfModel
                    };

                    FloorList.Add(crntFloor);
                }
            }

            return FloorList;
        }

    }
}