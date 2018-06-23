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
        public IfWall2 IfWall2 { get; set; }
        public IIfcLocalPlacement LocalPlacement { get; set; }
        #endregion

        #region Member Variables


        #endregion
        #region Constructor
        public IfSill() : base(null)
        {
        }
        public IfSill(IfWall wall) : base(wall.IfModel)
        {
            IfWall = wall;
        }
        public IfSill(IfWall2 wall) : base(wall.IfModel)
        {
            IfWall2 = wall;
        }

        #endregion
        #region Methods

        public void New()
        {
            var model = IfWall2.IfModel.IfcStore;
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
        public void New2()
        {
            var model = IfWall2.IfModel.IfcStore;
            // CheckUnits();
            using (var txn = model.BeginTransaction("New Sill"))
            {
                //beam proprties.
                IfcElement = model.Instances.New<IfcBeamStandardCase>();
                ((IfcBeamStandardCase)IfcElement).PredefinedType = IfcBeamTypeEnum.JOIST;
                IfcElement.ObjectType = "wood-Sill";
                SetLocation2(model);
                SetShape2(model);

                var story = IfWall2.IfcWall.ContainedInStructure.FirstOrDefault().RelatedElements;
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


        private void SetShape2(IfcStore ifcModel)
        {
            var rep = ifcModel.Instances.New<IfcProductDefinitionShape>();

            //#region Extruded Representation
            //var recProfile = ifcModel.Instances.OfType<IfcRectangleProfileDef>()
            //    .Where(e =>
            //    e.ProfileType == IfcProfileTypeEnum.AREA &&
            //    e.XDim == IfDimension.XDim.Feet &&
            //    e.YDim == IfDimension.YDim.Feet)
            //    .FirstOrDefault() ??
            //    ifcModel.Instances.New<IfcRectangleProfileDef>();
            //recProfile.ProfileType = IfcProfileTypeEnum.AREA;
            //recProfile.XDim = IfDimension.XDim.Feet;
            //recProfile.YDim = IfDimension.YDim.Feet;
            var recProfile = ifcModel.Instances.OfType<IfcRectangleProfileDef>()
                .Where(e=>e.XDim == IfDimension.XDim.Feet && 
                e.YDim == IfDimension.YDim.Feet).FirstOrDefault() ??
                ifcModel.Instances.New<IfcRectangleProfileDef>(e => 
                {
                    e.XDim = IfDimension.XDim.Feet;
                    e.YDim = IfDimension.YDim.Feet;
                    e.ProfileType = IfcProfileTypeEnum.AREA;
                });
            ////filling proerties eshta y3ny

            //var body = ifcModel.Instances.New<IfcExtrudedAreaSolid>();
            //body.Depth = IfDimension.ZDim.Feet;
            ////rectangle profile
            //body.SweptArea = recProfile;
            //body.ExtrudedDirection = ifcModel.Instances.OfType<IfcDirection>().Where(e =>
            //    e.X == 0 &&
            //    e.Y == 0 &&
            //    e.Z == 1).FirstOrDefault() ??
            //    ifcModel.Instances.New<IfcDirection>();
            //body.ExtrudedDirection.SetXYZ(0, 0, 1);

            var body = ifcModel.Instances.New<IfcExtrudedAreaSolid>();
            body.Depth = IfDimension.ZDim.Feet;
            //rectangle profile
            body.SweptArea = recProfile;
            body.ExtrudedDirection = ifcModel.Instances.OfType<IfcDirection>()
                .Where(e => e.X == 0 && e.Y == 0 && e.Z == 1 ).FirstOrDefault() ?? ifcModel.Instances.New<IfcDirection>
                ( e=> { e.SetXYZ(0, 0, 1); });
            //var point = ifcModel.Instances.OfType<IfcCartesianPoint>()
            //    .Where(e => e.X == 0 && e.Y == 0 && e.Z == 0).FirstOrDefault() ??
            //    ifcModel.Instances.New<IfcCartesianPoint>();
            //point.SetXYZ(0, 0, 0);

            var point = ifcModel.Instances.OfType<IfcCartesianPoint>().Where(e =>
                e.X == 0 && e.Y == 0 && e.Z == 0 ).FirstOrDefault() ??
                ifcModel.Instances.New<IfcCartesianPoint>(e =>{e.SetXYZ(0,0,0);});
            //body.Position = ifcModel.Instances.New<IfcAxis2Placement3D>();
            //body.Position.Location = point;
            //body.Position.RefDirection = ifcModel.Instances.OfType<IfcDirection>().Where(e =>
            //    e.X == 1 &&
            //    e.Y == 0 &&
            //    e.Z == 0)
            //.FirstOrDefault() ??
            //ifcModel.Instances.New<IfcDirection>();

            body.Position = ifcModel.Instances.New<IfcAxis2Placement3D>();
            body.Position.Location = point;
            body.Position.RefDirection = ifcModel.Instances.OfType<IfcDirection>()
                .Where(e => e.X == 1 && e.Y == 0 && e.Z == 0).FirstOrDefault() ?? ifcModel.Instances.New<IfcDirection>
                (e => { e.SetXYZ(1, 0, 0); });
            body.Position.Axis = ifcModel.Instances.OfType<IfcDirection>()
                .Where(e => e.X == 0 && e.Y == 0 && e.Z ==1).FirstOrDefault() ?? ifcModel.Instances.New<IfcDirection>
                (e => { e.SetXYZ(0, 0, 1); });
            //// placment.RefDirection.SetXYZ(xDir.X, xDir.Y,xDir.Z);
            //body.Position.RefDirection.SetXYZ(
            //    1,
            //    0,
            //    0
            //    );

            //body.Position.Axis = ifcModel.Instances.OfType<IfcDirection>().Where(e =>
            //    e.X == 0 &&
            //    e.Y == 0 &&
            //    e.Z == 1)
            //    .FirstOrDefault() ??
            //    ifcModel.Instances.New<IfcDirection>();

            //body.Position.Axis.SetXYZ(
            //    0,
            //    0,
            //    1
            //    );

            var shape = ifcModel.Instances.New<IfcShapeRepresentation>();
            var modelContext = ifcModel.Instances.OfType<IfcGeometricRepresentationContext>().FirstOrDefault();
            shape.ContextOfItems = modelContext;
            shape.RepresentationType = "SweptSolid";
            shape.RepresentationIdentifier = "Body";
            shape.Items.Add(body);
            rep.Representations.Add(shape);

            ////Create a Definition shape to hold the geometry
            //var shape = ifcModel.Instances.New<IfcShapeRepresentation>();
            //var modelContext = ifcModel.Instances.OfType<IfcGeometricRepresentationContext>().FirstOrDefault();
            //shape.ContextOfItems = modelContext;
            //shape.RepresentationType = "SweptSolid";
            //shape.RepresentationIdentifier = "Body";
            //shape.Items.Add(body);
            //rep.Representations.Add(shape);
            //#endregion


            //filling proerties eshta y3ny

            //parameters to insert the geometry in the model
            // var origin = ifcModel.Instances.New<IfcCartesianPoint>();

            //#region Axis Representation
            //// linear segment as IfcPolyline with two points is required for IfcWall
            //var ifcPolyline = ifcModel.Instances.New<IfcPolyline>();
            //var startPoint = ifcModel.Instances.New<IfcCartesianPoint>();
            //startPoint.SetXY(IfLocation.X.Feet, IfLocation.Y.Feet);
            //var endPoint = ifcModel.Instances.New<IfcCartesianPoint>();
            ///*          Set Stud Location */
            var ifcPolyline = ifcModel.Instances.New<IfcPolyline>();
            var startPoint = ifcModel.Instances.New<IfcCartesianPoint>();
            startPoint.SetXY(IfLocation.X.Feet, IfLocation.Y.Feet);
            var endPoint = ifcModel.Instances.New<IfcCartesianPoint>();
            //endPoint.SetXY(IfLocation.X.Feet + IfDimension.ZDim.Feet * IfDirection.X, IfLocation.Y.Feet + IfDimension.ZDim.Feet * IfDirection.Y);
            //ifcPolyline.Points.Add(startPoint);
            //ifcPolyline.Points.Add(endPoint);
            endPoint.SetXY(IfLocation.X + IfDimension.XDim.Feet, IfLocation.Y + IfDimension.YDim.Feet);
            ifcPolyline.Points.Add(startPoint);
            ifcPolyline.Points.Add(endPoint);

            //var shape2D = ifcModel.Instances.New<IfcShapeRepresentation>();
            //shape2D.ContextOfItems = modelContext;
            //shape2D.RepresentationIdentifier = "Axis";
            //shape2D.RepresentationType = "Curve2D";
            //shape2D.Items.Add(ifcPolyline);
            //rep.Representations.Add(shape2D);
            //#endregion

            //IfcElement.Representation = rep;



            //Create a Definition shape to hold the geometry


            // linear segment as IfcPolyline with two points is required for IfcWall

            /*          Set Stud Location */
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
            var origin = ifcModel.Instances.New<IfcCartesianPoint>();

            /*          Set Stud Location */
            origin.SetXYZ(IfLocation.X.Feet, IfLocation.Y.Feet, IfLocation.Z.Feet);

            var lp = ifcModel.Instances.New<IfcLocalPlacement>();
            var ax3D = ifcModel.Instances.New<IfcAxis2Placement3D>();
            /*          Set Stud Location */
            lp.PlacementRelTo = (IfcLocalPlacement)IfWall.LocalPlacement;
            ax3D.Location = origin;
            if (((IfcAxis2Placement3D)LocalPlacement.RelativePlacement).RefDirection != null)
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

            if (((IfcAxis2Placement3D)LocalPlacement.RelativePlacement).Axis != null)
            {
                ax3D.Axis = ((IfcAxis2Placement3D)LocalPlacement.RelativePlacement).Axis;
            }
            else
            {
                ax3D.Axis = ifcModel.Instances.OfType<IfcDirection>().Where
                    (e => e.X == 0 &&
                    e.Y == 0 &&
                    e.Z == 1).FirstOrDefault() ??
                    ifcModel.Instances.New<IfcDirection>();
                ax3D.Axis.SetXYZ(0, 0, 1);
            }

            lp.RelativePlacement = ax3D;

            IfcElement.ObjectPlacement = lp;
        }
        private void SetLocation2(IfcStore ifcModel)
        {

            //var JoistLocalPlacement = ifcModel.Instances.New<IfcLocalPlacement>();
            var SillLocalPlacement = ifcModel.Instances.New<IfcLocalPlacement>();
            //var JoistRelativePlacment = ifcModel.Instances.New<IfcAxis2Placement3D>();
            ////
            //// setting the location
            ////
            //var RelativeZ = Length.FromInches(IfLocation.Z.Inches - IfDimension.YDim / 2);
            //#region relative location
            //JoistRelativePlacment.Location = ifcModel.Instances.OfType<IfcCartesianPoint>().Where(
            //    e => e.X == IfLocation.X.Feet &&
            //    e.Y == IfLocation.Y.Feet &&
            //    e.Z == RelativeZ.Feet)
            //    .FirstOrDefault() ??
            //    ifcModel.Instances.New<IfcCartesianPoint>();

            //JoistRelativePlacment.Location.SetXYZ(
            //    IfLocation.X.Feet,
            //    IfLocation.Y.Feet,
            //    RelativeZ.Feet
            //    );

            var SillPlacementRelTo = ifcModel.Instances.New<IfcLocalPlacement>();
            var SillRelativePlacementLocation = ifcModel.Instances.OfType<IfcCartesianPoint>().Where(e =>
                e.X == IfLocation.X && e.Y == IfLocation.Y && e.Z == IfLocation.Z //&&
                ).FirstOrDefault() ??
                ifcModel.Instances.New<IfcCartesianPoint>(e =>
                {
                    e.X = IfLocation.X; e.Y = IfLocation.Y; e.Z = IfLocation.Z;
                });

            //JoistRelativePlacment.Axis = ifcModel.Instances.OfType<IfcDirection>().Where(e =>
            //    e.X == zDir.X &&
            //    e.Y == zDir.Y &&
            //    e.Z == zDir.Z)
            //    .FirstOrDefault() ??
            //    ifcModel.Instances.New<IfcDirection>();

            //JoistRelativePlacment.Axis.SetXYZ(
            //    zDir.X,
            //    zDir.Y,
            //    zDir.Z
            //    );

            var Axis = ifcModel.Instances.OfType<IfcDirection>().Where(e =>
                e.X == IfWall2.IfDirection.X && e.Y == IfWall2.IfDirection.Y && e.Z == IfWall2.IfDirection.Z //&&
                ).FirstOrDefault() ??
                ifcModel.Instances.New<IfcDirection>(e =>
                {
                    e.X = IfWall2.IfDirection.X; e.Y = IfWall2.IfDirection.Y; e.Z = IfWall2.IfDirection.Z;
                });

            //JoistRelativePlacment.RefDirection = ifcModel.Instances.OfType<IfcDirection>().Where(e =>
            //    e.X == nDir.X &&
            //    e.Y == nDir.Y &&
            //    e.Z == nDir.Z)
            //    .FirstOrDefault() ??
            //    ifcModel.Instances.New<IfcDirection>();

            var RefDirection = ifcModel.Instances.OfType<IfcDirection>().Where(e =>
                e.X == 0 && e.Y == 0 && e.Z == 1 //&&
                ).FirstOrDefault() ??
                ifcModel.Instances.New<IfcDirection>(e =>
                {
                    e.X = 0; e.Y = 0; e.Z = 1;
                });

            var SillRelativePlacement = ifcModel.Instances.New<IfcAxis2Placement3D>
                (e =>
                {
                    e.Location = SillRelativePlacementLocation;
                    e.Axis = Axis;
                    e.RefDirection = RefDirection;
                });





            SillPlacementRelTo = (IfcLocalPlacement)IfWall2.IfcWall.ObjectPlacement;
            //SillPlacementRelTo = (IfcLocalPlacement)IfWall2.;
            SillLocalPlacement.PlacementRelTo = SillPlacementRelTo;
            SillLocalPlacement.RelativePlacement = SillRelativePlacement;

            IfcElement.ObjectPlacement = SillLocalPlacement;
        }


        //private void SetLocation3(IfcStore ifcModel)
        //{

        //    // placment.RefDirection.SetXYZ(xDir.X, xDir.Y,xDir.Z);

        //    //placment.Axis.SetXYZ(
        //    //    0,0,1
        //    //    );

        //    JoistLocalPlacement.RelativePlacement = JoistRelativePlacment;
        //    #endregion

        //    #region intermideate local placement
        //    var intermideateLocalPLacement = ifcModel.Instances.New<IfcLocalPlacement>();
        //    var intermideatePlacementRelTo = ifcModel.Instances.New<IfcLocalPlacement>();
        //    var intermideateRelativePlacment = ifcModel.Instances.New<IfcAxis2Placement3D>();
        //    intermideateRelativePlacment.Location = ifcModel.Instances.OfType<IfcCartesianPoint>()
        //        .Where(e =>
        //            e.X == IfFloor.IfLocation.X.Feet &&
        //            e.Y == IfFloor.IfLocation.Y.Feet &&
        //            e.Z == IfFloor.IfLocation.Z.Feet)
        //        .FirstOrDefault() ??
        //        ifcModel.Instances.New<IfcCartesianPoint>();

        //    intermideateRelativePlacment.Location.X = IfFloor.IfLocation.X.Feet;
        //    intermideateRelativePlacment.Location.Y = IfFloor.IfLocation.Y.Feet;
        //    intermideateRelativePlacment.Location.Z = IfFloor.IfLocation.Z.Feet;
        //    //intermideateRelativePlacment.Location.Z = 0;

        //    intermideateRelativePlacment.Axis =
        //        ifcModel.Instances.OfType<IfcDirection>().Where
        //        (e => e.X == 0 &&
        //        e.Y == 0 &&
        //        e.Z == 1).FirstOrDefault() ??
        //        ifcModel.Instances.New<IfcDirection>();
        //    intermideateRelativePlacment.Axis.SetXYZ(0, 0, 1);

        //    intermideateRelativePlacment.RefDirection =
        //        ifcModel.Instances.OfType<IfcDirection>().Where
        //        (e => e.X == 1 &&
        //        e.Y == 0 &&
        //        e.Z == 0).FirstOrDefault() ??
        //        ifcModel.Instances.New<IfcDirection>();
        //    intermideateRelativePlacment.RefDirection.SetXYZ(1, 0, 0);

        //    var storyLocalPlacement = (IfcLocalPlacement)(((IfcLocalPlacement)IfFloor.IfcSlab.ObjectPlacement).PlacementRelTo);
        //    //var storyLocalPlacement = ((IfcLocalPlacement)IfFloor.IfcSlab.ObjectPlacement);
        //    intermideatePlacementRelTo = storyLocalPlacement;
        //    intermideateLocalPLacement.RelativePlacement = intermideateRelativePlacment;
        //    intermideateLocalPLacement.PlacementRelTo = intermideatePlacementRelTo;
        //    #endregion

        //    //IfcAxis2Placement3D OriginRelativePlacment = ifcModel.Instances.OfType<IfcAxis2Placement3D>()
        //    //    .Where(e => e.Location.X == 0 && e.Location.Y == 0 && e.Location.Z == 0).FirstOrDefault();
        //    //IfcLocalPlacement Origin = ifcModel.Instances.OfType<IfcLocalPlacement>()
        //    //    .Where(
        //    //    e => e.RelativePlacement == OriginRelativePlacment
        //    //    ).FirstOrDefault();

        //    JoistLocalPlacement.PlacementRelTo = intermideateLocalPLacement;

        //    //setting the relativ placement
        //    IfcElement.ObjectPlacement = JoistLocalPlacement;
        //}

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
