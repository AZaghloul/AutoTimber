using System.Linq;
using Xbim.Ifc4.Interfaces;
using Xbim.Ifc4.ProfileResource;
using Xbim.Ifc;
using Xbim.Ifc4.GeometryResource;
using Xbim.Ifc4.RepresentationResource;
using Xbim.Ifc4.SharedBldgElements;
using Xbim.Ifc4.GeometricConstraintResource;
using Xbim.Ifc4.ProductExtension;
using Xbim.Ifc4.GeometricModelResource;
using Bim.Domain.Configuration;
using MathNet.Spatial.Euclidean;
using System;

namespace Bim.Domain.Ifc
{
    public class IfJoist : IfElement
    {
        #region Properties

        public IfFloor IfFloor { get; set; }
        public IfcAxis2Placement3D RelativeAxis { get; set; }
        public IfcLocalPlacement LocalPlacement { get; set; }
        public IfDirection IfDirection { get; set; }
        #endregion
        public static Setup Setup { get; set; }

        public IfJoist() : base(null)
        {
        }
        public IfJoist(IfFloor floor) : base(floor.IfModel)
        {
            IfFloor = floor;
            IfDirection = floor.ShortDirection;
        }

        public void New()
        {
            var model = IfFloor.IfModel.IfcStore;
            // CheckUnits();
            using (var txn = model.BeginTransaction("New Sill"))
            {
                //beam proprties.
                IfcElement = model.Instances.New<IfcBeamStandardCase>();
                ((IfcBeamStandardCase)IfcElement).PredefinedType = IfcBeamTypeEnum.JOIST;
                IfcElement.ObjectType = "Slab-Joist";
                SetLocation(model);
                SetShape(model);

                var story = IfFloor.IfcSlab.ContainedInStructure.FirstOrDefault().RelatedElements;
                story.Add(IfcElement);
                txn.Commit();
            }
        }

        #region Helper Method
        private void SetShape(IfcStore ifcModel)
        {
            var rep = ifcModel.Instances.New<IfcProductDefinitionShape>();

            #region Extruded Representation
            var recProfile = ifcModel.Instances.OfType<IfcRectangleProfileDef>()
                .Where(e =>
                e.ProfileType == IfcProfileTypeEnum.AREA &&
                e.XDim == IfDimension.XDim.Feet &&
                e.YDim == IfDimension.YDim.Feet)
                .FirstOrDefault() ??
                ifcModel.Instances.New<IfcRectangleProfileDef>();
            recProfile.ProfileType = IfcProfileTypeEnum.AREA;
            recProfile.XDim = IfDimension.XDim.Feet;
            recProfile.YDim = IfDimension.YDim.Feet;
            //filling proerties eshta y3ny

            var body = ifcModel.Instances.New<IfcExtrudedAreaSolid>();
            body.Depth = IfDimension.ZDim.Feet;
            //rectangle profile
            body.SweptArea = recProfile;
            body.ExtrudedDirection = ifcModel.Instances.OfType<IfcDirection>().Where(e =>
                e.X == 0 &&
                e.Y == 0 &&
                e.Z == 1).FirstOrDefault() ??
                ifcModel.Instances.New<IfcDirection>();
            body.ExtrudedDirection.SetXYZ(0, 0, 1);

            var point = ifcModel.Instances.OfType<IfcCartesianPoint>()
                .Where(e => e.X == 0 && e.Y == 0 && e.Z == 0).FirstOrDefault() ??
                ifcModel.Instances.New<IfcCartesianPoint>();
            point.SetXYZ(0, 0, 0);

            body.Position = ifcModel.Instances.New<IfcAxis2Placement3D>();
            body.Position.Location = point;
            body.Position.RefDirection = ifcModel.Instances.OfType<IfcDirection>().Where(e =>
                e.X == 1 &&
                e.Y == 0 &&
                e.Z == 0)
            .FirstOrDefault() ??
            ifcModel.Instances.New<IfcDirection>();

            // placment.RefDirection.SetXYZ(xDir.X, xDir.Y,xDir.Z);
            body.Position.RefDirection.SetXYZ(
                1,
                0,
                0
                );

            body.Position.Axis = ifcModel.Instances.OfType<IfcDirection>().Where(e =>
                e.X == 0 &&
                e.Y == 0 &&
                e.Z == 1)
                .FirstOrDefault() ??
                ifcModel.Instances.New<IfcDirection>();

            body.Position.Axis.SetXYZ(
                0,
                0,
                1
                );


            //Create a Definition shape to hold the geometry
            var shape = ifcModel.Instances.New<IfcShapeRepresentation>();
            var modelContext = ifcModel.Instances.OfType<IfcGeometricRepresentationContext>().FirstOrDefault();
            shape.ContextOfItems = modelContext;
            shape.RepresentationType = "SweptSolid";
            shape.RepresentationIdentifier = "Body";
            shape.Items.Add(body);
            rep.Representations.Add(shape);
            #endregion

            #region Axis Representation
            // linear segment as IfcPolyline with two points is required for IfcWall
            var ifcPolyline = ifcModel.Instances.New<IfcPolyline>();
            var startPoint = ifcModel.Instances.New<IfcCartesianPoint>();
            startPoint.SetXY(IfLocation.X.Feet, IfLocation.Y.Feet);
            var endPoint = ifcModel.Instances.New<IfcCartesianPoint>();
            /*          Set Stud Location */
            endPoint.SetXY(IfLocation.X.Feet + IfDimension.ZDim.Feet * IfDirection.X, IfLocation.Y.Feet + IfDimension.ZDim.Feet * IfDirection.Y);
            ifcPolyline.Points.Add(startPoint);
            ifcPolyline.Points.Add(endPoint);
            var shape2D = ifcModel.Instances.New<IfcShapeRepresentation>();
            shape2D.ContextOfItems = modelContext;
            shape2D.RepresentationIdentifier = "Axis";
            shape2D.RepresentationType = "Curve2D";
            shape2D.Items.Add(ifcPolyline);
            rep.Representations.Add(shape2D);
            #endregion

            IfcElement.Representation = rep;

        }
        private void SetLocation(IfcStore ifcModel)
        {

            var JoistLocalPlacement = ifcModel.Instances.New<IfcLocalPlacement>();
            var JoistRelativePlacment = ifcModel.Instances.New<IfcAxis2Placement3D>();
            //
            // setting the location
            //

            #region relative location
            JoistRelativePlacment.Location = ifcModel.Instances.OfType<IfcCartesianPoint>().Where(
                e => e.X == IfLocation.X.Feet &&
                e.Y == IfLocation.Y.Feet &&
                e.Z == IfLocation.Z.Feet)
                .FirstOrDefault() ??
                ifcModel.Instances.New<IfcCartesianPoint>();

            JoistRelativePlacment.Location.SetXYZ(
                IfLocation.X.Feet,
                IfLocation.Y.Feet,
                IfLocation.Z.Feet
                );

            var zDir = new Vector3D(
                IfFloor.ShortDirection.X,
                IfFloor.ShortDirection.Y,
                IfFloor.ShortDirection.Z);

            var nDir = new Vector3D(
                IfFloor.LongDirection.X,
                IfFloor.LongDirection.Y,
                IfFloor.LongDirection.Z
                );
            var xDir = zDir.CrossProduct(nDir);

            JoistRelativePlacment.RefDirection = ifcModel.Instances.OfType<IfcDirection>().Where(e =>
                e.X == nDir.X &&
                e.Y == nDir.Y &&
                e.Z == nDir.Z)
                .FirstOrDefault() ??
                ifcModel.Instances.New<IfcDirection>();

            // placment.RefDirection.SetXYZ(xDir.X, xDir.Y,xDir.Z);
            JoistRelativePlacment.RefDirection.SetXYZ(
                nDir.X,
                nDir.Y,
                nDir.Z
                );

            JoistRelativePlacment.Axis = ifcModel.Instances.OfType<IfcDirection>().Where(e =>
                e.X == zDir.X &&
                e.Y == zDir.Y &&
                e.Z == zDir.Z)
                .FirstOrDefault() ??
                ifcModel.Instances.New<IfcDirection>();

            JoistRelativePlacment.Axis.SetXYZ(
                zDir.X,
                zDir.Y,
                zDir.Z
                );

            //placment.Axis.SetXYZ(
            //    0,0,1
            //    );

            JoistLocalPlacement.RelativePlacement = JoistRelativePlacment;
            #endregion

            #region intermideate local placement
            var intermideateLocalPLacement = ifcModel.Instances.New<IfcLocalPlacement>();
            var intermideatePlacementRelTo = ifcModel.Instances.New<IfcLocalPlacement>();
            var intermideateRelativePlacment = ifcModel.Instances.New<IfcAxis2Placement3D>();
            intermideateRelativePlacment.Location = ifcModel.Instances.OfType<IfcCartesianPoint>()
                .Where(e =>
                    e.X == IfFloor.IfLocation.X.Feet &&
                    e.Y == IfFloor.IfLocation.Y.Feet &&
                    e.Z == IfFloor.IfLocation.Z.Feet)
                .FirstOrDefault() ??
                ifcModel.Instances.New<IfcCartesianPoint>();

            intermideateRelativePlacment.Location.X = IfFloor.IfLocation.X.Feet;
            intermideateRelativePlacment.Location.Y = IfFloor.IfLocation.Y.Feet;
            intermideateRelativePlacment.Location.Z = IfFloor.IfLocation.Z.Feet;
            //intermideateRelativePlacment.Location.Z = 0;

            intermideateRelativePlacment.Axis =
                ifcModel.Instances.OfType<IfcDirection>().Where
                (e => e.X == 0 &&
                e.Y == 0 &&
                e.Z == 1).FirstOrDefault() ??
                ifcModel.Instances.New<IfcDirection>();
            intermideateRelativePlacment.Axis.SetXYZ(0, 0, 1);

            intermideateRelativePlacment.RefDirection =
                ifcModel.Instances.OfType<IfcDirection>().Where
                (e => e.X == 1 &&
                e.Y == 0 &&
                e.Z == 0).FirstOrDefault() ??
                ifcModel.Instances.New<IfcDirection>();
            intermideateRelativePlacment.RefDirection.SetXYZ(1, 0, 0);

            var storyLocalPlacement = (IfcLocalPlacement)(((IfcLocalPlacement)IfFloor.IfcSlab.ObjectPlacement).PlacementRelTo);
            //var storyLocalPlacement = ((IfcLocalPlacement)IfFloor.IfcSlab.ObjectPlacement);
            intermideatePlacementRelTo = storyLocalPlacement;
            intermideateLocalPLacement.RelativePlacement = intermideateRelativePlacment;
            intermideateLocalPLacement.PlacementRelTo = intermideatePlacementRelTo;
            #endregion

            //IfcAxis2Placement3D OriginRelativePlacment = ifcModel.Instances.OfType<IfcAxis2Placement3D>()
            //    .Where(e => e.Location.X == 0 && e.Location.Y == 0 && e.Location.Z == 0).FirstOrDefault();
            //IfcLocalPlacement Origin = ifcModel.Instances.OfType<IfcLocalPlacement>()
            //    .Where(
            //    e => e.RelativePlacement == OriginRelativePlacment
            //    ).FirstOrDefault();

            JoistLocalPlacement.PlacementRelTo = intermideateLocalPLacement;

            //setting the relativ placement
            IfcElement.ObjectPlacement = JoistLocalPlacement;
        }

        #endregion

        public override string ToString()
        {
            return $"Floor Joist {IfDimension.XDim.Inches} × {IfDimension.YDim.Inches} × {Math.Round(IfDimension.ZDim.Inches, 0)}";
        }
    }
}