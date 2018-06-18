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

namespace Bim.Domain.Ifc
{
    public class IfJoist : IfElement
    {
        #region Properties

        public IfFloor IfFloor { get; set; }
        public IfcAxis2Placement3D RelativeAxis { get; set; }
        public IfcLocalPlacement LocalPlacement { get; set; }
        #endregion
        public static Setup Setup { get; set; }

        public IfJoist()
        {
        }
        public IfJoist(IfFloor floor)
        {
            IfFloor = floor;
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

            var recProfile = ifcModel.Instances.New<IfcRectangleProfileDef>();

            //filling proerties eshta y3ny
            recProfile.ProfileType = IfcProfileTypeEnum.AREA;
            recProfile.XDim = IfDimension.XDim / 12;
            recProfile.YDim = IfDimension.YDim / 12;

            var body = ifcModel.Instances.New<IfcExtrudedAreaSolid>();
            body.Depth = IfDimension.ZDim;
            //rectangle profile
            body.SweptArea = recProfile;
            body.ExtrudedDirection = ifcModel.Instances.New<IfcDirection>();
            body.ExtrudedDirection.SetXYZ(0, 0, 1);
            //  body.ExtrudedDirection.SetXYZ();


            var point = ifcModel.Instances.New<IfcCartesianPoint>();
            point.SetXYZ(0, 0, 0);
            body.Position = ifcModel.Instances.New<IfcAxis2Placement3D>();
            body.Position.Location = point;

            //Create a Definition shape to hold the geometry
            var shape = ifcModel.Instances.New<IfcShapeRepresentation>();
            var modelContext = ifcModel.Instances.OfType<IfcGeometricRepresentationContext>().FirstOrDefault();
            shape.ContextOfItems = modelContext;
            shape.RepresentationType = "SweptSolid";
            shape.RepresentationIdentifier = "Body";
            shape.Items.Add(body);
            var rep = ifcModel.Instances.New<IfcProductDefinitionShape>();
            rep.Representations.Add(shape);
            // linear segment as IfcPolyline with two points is required for IfcWall
            var ifcPolyline = ifcModel.Instances.New<IfcPolyline>();
            var startPoint = ifcModel.Instances.New<IfcCartesianPoint>();
            startPoint.SetXY(IfLocation.X, IfLocation.Y);
            var endPoint = ifcModel.Instances.New<IfcCartesianPoint>();
            /*          Set Stud Location */
            endPoint.SetXY(IfLocation.X + IfDimension.XDim, IfLocation.Y + IfDimension.YDim);
            ifcPolyline.Points.Add(startPoint);
            ifcPolyline.Points.Add(endPoint);

            var shape2D = ifcModel.Instances.New<IfcShapeRepresentation>();
            shape2D.ContextOfItems = modelContext;
            shape2D.RepresentationIdentifier = "Axis";
            shape2D.RepresentationType = "Curve2D";
            shape2D.Items.Add(ifcPolyline);
            rep.Representations.Add(shape2D);
            IfcElement.Representation = rep;
        }
        private void SetLocation(IfcStore ifcModel)
        {
            var lp = ifcModel.Instances.New<IfcLocalPlacement>();
            var relPlacement = ifcModel.Instances.New<IfcLocalPlacement>();

            var placment = ifcModel.Instances.New<IfcAxis2Placement3D>();
            //
            // setting the location
            //
            placment.Location = ifcModel.Instances.New<IfcCartesianPoint>();
            placment.Location.SetXYZ(
                IfLocation.X,
               IfLocation.Y,
              IfLocation.Z
                );
            placment.RefDirection = ifcModel.Instances.New<IfcDirection>();
            placment.Axis = ifcModel.Instances.New<IfcDirection>();
            placment.Axis.SetXYZ(
                IfFloor.ShortDirection.X,
                IfFloor.ShortDirection.Y,
                IfFloor.ShortDirection.Z
                );
            //placment.Axis.SetXYZ(
            //    0,0,1
            //    );
            placment.RefDirection.SetXYZ(1, 0, 0);
            lp.RelativePlacement = placment;

            //setting the relativ placement
            lp.PlacementRelTo = relPlacement;
            relPlacement.PlacementRelTo = ((IfcLocalPlacement)IfFloor.IfcSlab.ObjectPlacement).PlacementRelTo;

            var loc = (IfcCartesianPoint)IfFloor.PolyLine.Points[0];
            relPlacement.RelativePlacement = ifcModel.Instances.New<IfcAxis2Placement3D>();
            ((IfcAxis2Placement3D)relPlacement.RelativePlacement).Location = (IfcCartesianPoint)IfFloor.PolyLine.Points[0];

            IfcElement.ObjectPlacement = lp;
        }
        private void CheckUnits()
        {
            var unit = IfModel.IfUnit.LengthUnit;
            switch (unit)
            {
                case UnitName.METRE:
                    IfDimension = IfDimension.ToMeters();
                    IfLocation = IfLocation.ToMeters();
                    break;

                case UnitName.MILLIMETRE:
                    IfDimension = IfDimension.ToMilliMeters();
                    IfLocation = IfLocation.ToMilliMeters();
                    break;

                case UnitName.FOOT:
                    break;
                default:
                    break;
            }
        }
        #endregion


    }
}