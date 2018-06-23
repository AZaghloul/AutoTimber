using System.Linq;
using Xbim.Ifc4.Interfaces;
using Xbim.Ifc4.ProfileResource;
using Xbim.Ifc;
using Xbim.Ifc4.GeometryResource;
using Xbim.Ifc4.GeometricModelResource;
using Xbim.Ifc4.RepresentationResource;
using Xbim.Ifc4.SharedBldgElements;
using Xbim.Ifc4.GeometricConstraintResource;
using Xbim.Ifc4.ProductExtension;

using Xbim.Ifc4.Kernel;
using Xbim.Ifc4.PropertyResource;
using Bim.Domain.General;

namespace Bim.Domain.Ifc
{
    public class IfSill : IfElement
    {
        public static Setup Setup { get; set; }
        #region Properties
        public IfWall IfWall { get; set; }
        public IIfcLocalPlacement LocalPlacement { get; set; }
        #endregion

        #region Member Variables


        #endregion
        #region Constructor
        public IfSill():base(null)
        {
        }
        public IfSill(IfWall wall):base(wall.IfModel)
        {
            IfWall = wall;
        }

        #endregion
        #region Methods

        public void New()
        {
            var model = IfWall.IfModel.IfcStore;
            // CheckUnits();
            using (var txn = model.BeginTransaction("New Sill"))
            {
                //beam proprties.
                IfcElement = model.Instances.New<IfcBeamStandardCase>();
                ((IfcBeamStandardCase)IfcElement).PredefinedType = IfcBeamTypeEnum.JOIST;
                IfcElement.ObjectType = "wood-Sill";
                SetLocation(model);
                SetShape(model);

                var story = IfWall.IfcWall.ContainedInStructure.FirstOrDefault().RelatedElements;
                story.Add(IfcElement);
                txn.Commit();
            }
        }

        #endregion

        #region Helper Method
        private void SetShape(IfcStore ifcModel)
        {

            var recProfile = ifcModel.Instances.New<IfcRectangleProfileDef>();

            //filling proerties eshta y3ny
            recProfile.ProfileType = IfcProfileTypeEnum.AREA;
            recProfile.XDim = IfDimension.XDim.Feet;
            recProfile.YDim = IfDimension.YDim.Feet;

            var body = ifcModel.Instances.New<IfcExtrudedAreaSolid>();
            body.Depth = IfDimension.ZDim.Feet;
            //rectangle profile
            body.SweptArea = recProfile;
            body.ExtrudedDirection = ifcModel.Instances.New<IfcDirection>();
            body.ExtrudedDirection.SetXYZ(0, 0, 1);
            //parameters to insert the geometry in the model
            // var origin = ifcModel.Instances.New<IfcCartesianPoint>();

            /*          Set Stud Location */
            //origin.SetXYZ(IfLocation.X, IfLocation.Y, IfLocation.Z);

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
            endPoint.SetXY(IfLocation.X + IfDimension.XDim.Inches, IfLocation.Y + IfDimension.YDim.Inches);
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
            //parameters to insert the geometry in the model
            var origin = ifcModel.Instances.New<IfcCartesianPoint>();

            /*          Set Stud Location */
            origin.SetXYZ(IfLocation.X.Feet, IfLocation.Y.Feet, IfLocation.Z.Feet);

            var lp = ifcModel.Instances.New<IfcLocalPlacement>();
            var ax3D = ifcModel.Instances.New<IfcAxis2Placement3D>();
            /*          Set Stud Location */
      
            ax3D.Location = origin;

            var refDirc = ((IfcAxis2Placement3D)LocalPlacement.RelativePlacement).RefDirection;
            var axisDirc=((IfcAxis2Placement3D)LocalPlacement.RelativePlacement).Axis;
            if (refDirc!=null)
            {
                ax3D.RefDirection = ((IfcAxis2Placement3D)LocalPlacement.RelativePlacement).RefDirection;
            }

            else
            {
               
                ax3D.RefDirection = ifcModel.Instances.OfType<IfcDirection>().Where
                    (e => e.X == 1 &&
                    e.Y == 0 &&
                    e.Z == 0).FirstOrDefault() ??
                    ifcModel.Instances.New<IfcDirection>();
                ax3D.RefDirection.SetXYZ(1, 0, 0);// ((IfcAxis2Placement3D)LocalPlacement.RelativePlacement).RefDirection; //x-axis direction
            }

            if ( axisDirc !=null)
            {
                ax3D.Axis = ((IfcAxis2Placement3D)LocalPlacement.RelativePlacement).Axis;
            }
            else
            {
                ax3D.Axis = ifcModel.Instances.OfType<IfcDirection>().Where
                    (e=> e.X == 0 &&
                    e.Y == 0 &&
                    e.Z == 1).FirstOrDefault() ??
                    ifcModel.Instances.New<IfcDirection>();
                ax3D.Axis.SetXYZ(0, 0, 1);
            }

            lp.RelativePlacement = ax3D;
            if (axisDirc!=null && refDirc !=null)
            {
                lp.PlacementRelTo = (IfcLocalPlacement)IfWall.LocalPlacement;
                ax3D.RefDirection = ifcModel.Instances.OfType<IfcDirection>().Where
                     (e => e.X == 1 &&
                     e.Y == 0 &&
                     e.Z == 0).FirstOrDefault() ??
                     ifcModel.Instances.New<IfcDirection>();
                ax3D.RefDirection.SetXYZ(1, 0, 0);// 
            }
            else
            {
                lp.PlacementRelTo = (IfcLocalPlacement)IfWall.LocalPlacement;
            }

            IfcElement.ObjectPlacement = lp;
        }
        /*private void CheckUnits()
        {
            var unit = IfModel.IfUnit.LengthUnit;
            switch (unit)
            {
                case UnitName.METRE:
                    IfDimension = IfDimension;
                    IfLocation = IfLocation;
                    break;

                case UnitName.MILLIMETRE:
                    IfDimension = IfDimension;
                    IfLocation = IfLocation;
                    break;

                case UnitName.FOOT:
                    break;
                default:
                    break;
            }
        }*/

        private void SetProperties()
        {
        }
        #endregion

        public override string ToString()
        {
            return $"Wall Sill {IfDimension.XDim.Inches} × {IfDimension.YDim.Inches} × {IfDimension.ZDim.Inches}";
        }

    }
}
