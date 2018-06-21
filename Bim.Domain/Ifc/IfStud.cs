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
using System;
using Bim.Common.Measures;

namespace Bim.Domain.Ifc
{
    public class IfStud : IfElement
    {
        #region Properties

        public IfWall IfWall { get; set; }
        public IfcAxis2Placement3D RelativeAxis { get; set; }
        public IfcLocalPlacement LocalPlacement { get; set; }
        #endregion
        public static Setup Setup { get; set; }
        #region Constructor
        public IfStud() : base(null)
        {
        }
        public IfStud(IfWall wall) : base(wall.IfModel)
        {
            IfWall = wall;
        }


        #endregion

        #region Methods

        public void New()
        {
            var ifcModel = IfWall.IfModel.IfcStore;
            // CheckUnits();

            using (var txn = ifcModel.BeginTransaction("New Stud"))
            {
                IfcElement = ifcModel.Instances.New<IfcColumnStandardCase>();
                //   IfcElement.Name = Name;
                ((IfcColumnStandardCase)IfcElement).PredefinedType = IfcColumnTypeEnum.COLUMN;
                SetLocation(ifcModel);
                SetShape(ifcModel);

                var story = IfWall.IfcWall.ContainedInStructure.FirstOrDefault().RelatedElements;
                story.Add(IfcElement);
                txn.Commit();
            }

        }
        #endregion
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

            //var ifcPolyline = ifcModel.Instances.New<IfcPolyline>();
            //var startPoint = ifcModel.Instances.New<IfcCartesianPoint>();
            //startPoint.SetXY(IfLocation.X, IfLocation.Y);
            //var endPoint = ifcModel.Instances.New<IfcCartesianPoint>();
            ///*          Set Stud Location */
            //endPoint.SetXY(IfLocation.X + IfDimension.XDim, IfLocation.Y + IfDimension.YDim);
            //ifcPolyline.Points.Add(startPoint);
            //ifcPolyline.Points.Add(endPoint);

            //var shape2D = ifcModel.Instances.New<IfcShapeRepresentation>();
            //shape2D.ContextOfItems = modelContext;
            //shape2D.RepresentationIdentifier = "Axis";
            //shape2D.RepresentationType = "Curve2D";
            //shape2D.Items.Add(ifcPolyline);
            //rep.Representations.Add(shape2D);
            IfcElement.Representation = rep;
            //var recProfile = ifcModel.Instances.New<IfcRectangleProfileDef>();

            ////filling proerties eshta y3ny
            //recProfile.ProfileType = IfcProfileTypeEnum.AREA;
            //recProfile.XDim = IfDimension.XDim;
            //recProfile.YDim = IfDimension.YDim;

            //var body = ifcModel.Instances.New<IfcExtrudedAreaSolid>();
            //body.Depth = IfDimension.ZDim;
            ////rectangle profile
            //body.SweptArea = recProfile;
            //body.ExtrudedDirection = ifcModel.Instances.New<IfcDirection>();
            //body.ExtrudedDirection.SetXYZ(0, 0, 1);
            ////parameters to insert the geometry in the model
            //// var origin = ifcModel.Instances.New<IfcCartesianPoint>();

            ///*          Set Stud Location */
            ////origin.SetXYZ(IfLocation.X, IfLocation.Y, IfLocation.Z);

            //var point = ifcModel.Instances.New<IfcCartesianPoint>();
            //point.SetXYZ(0, 0, 0);

            //body.Position = ifcModel.Instances.New<IfcAxis2Placement3D>();
            //body.Position.Location = point;

            ////Create a Definition shape to hold the geometry
            //var shape = ifcModel.Instances.New<IfcShapeRepresentation>();
            //var modelContext = ifcModel.Instances.OfType<IfcGeometricRepresentationContext>().FirstOrDefault();
            //shape.ContextOfItems = modelContext;
            //shape.RepresentationType = "SweptSolid";
            //shape.RepresentationIdentifier = "Body";
            //shape.Items.Add(body);

            //var rep = ifcModel.Instances.New<IfcProductDefinitionShape>();
            //rep.Representations.Add(shape);

            //// linear segment as IfcPolyline with two points is required for IfcWall

            //var ifcPolyline = ifcModel.Instances.New<IfcPolyline>();
            //var startPoint = ifcModel.Instances.New<IfcCartesianPoint>();
            //startPoint.SetXY(IfLocation.X, IfLocation.Y);
            //var endPoint = ifcModel.Instances.New<IfcCartesianPoint>();
            ///*          Set Stud Location */
            //endPoint.SetXY(IfLocation.X + IfDimension.XDim, IfLocation.Y + IfDimension.YDim);
            //ifcPolyline.Points.Add(startPoint);
            //ifcPolyline.Points.Add(endPoint);

            //var shape2D = ifcModel.Instances.New<IfcShapeRepresentation>();
            //shape2D.ContextOfItems = modelContext;
            //shape2D.RepresentationIdentifier = "Axis";
            //shape2D.RepresentationType = "Curve2D";
            //shape2D.Items.Add(ifcPolyline);
            //rep.Representations.Add(shape2D);
            //////
            //var shapeMap = ifcModel.Instances.New<IfcRepresentationMap>();
            //shapeMap.MappedRepresentation = shape;
            //shapeMap.MappingOrigin= ifcModel.Instances.New<IfcAxis2Placement3D>(); 
            //shape.ContextOfItems = modelContext;
            //shape.RepresentationType = "MappedRepresentation";
            //shape.RepresentationIdentifier = "Body";
            //shape.Items.Add(body);

            //rep.Representations.Add(shapeMap);
            //IfcElement.Representation = rep;


        }
        private void SetLocation(IfcStore ifcModel)
        {
            var origin = ifcModel.Instances.New<IfcCartesianPoint>();
            origin.SetXYZ(IfLocation.X.Feet, IfLocation.Y.Feet, IfLocation.Z.Feet);

            var ax3D = ifcModel.Instances.New<IfcAxis2Placement3D>();
            ax3D.Location = origin;
            ax3D.RefDirection = ifcModel.Instances.OfType<IfcDirection>().Where(e => e.X == 1 && e.Y == 0 && e.Z == 0).FirstOrDefault() ?? ifcModel.Instances.New<IfcDirection>();
            ax3D.RefDirection.SetXYZ(1, 0, 0);
            ax3D.Axis = ifcModel.Instances.OfType<IfcDirection>().Where(e => e.X == 0 && e.Y == 0 && e.Z == 1).FirstOrDefault() ?? ifcModel.Instances.New<IfcDirection>();
            ax3D.Axis.SetXYZ(0, 0, 1);

            /*          Set Stud Location */
            var lp = ifcModel.Instances.New<IfcLocalPlacement>();
            lp.PlacementRelTo = (IfcLocalPlacement)IfWall.LocalPlacement;
            lp.RelativePlacement = ax3D;
            IfcElement.ObjectPlacement = lp;

        }
        private void CheckUnits()
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
        }

        public override string ToString()
        {
            return $"Wall Stud {Math.Round(IfDimension.XDim.Inches, 0)} × {Math.Round(IfDimension.YDim.Inches)} × {Math.Round(IfDimension.ZDim.Inches)}";
        }

    }
}