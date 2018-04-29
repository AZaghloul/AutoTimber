using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xbim.Ifc4.Interfaces;
using Xbim.Ifc4.ProfileResource;
using Xbim.Ifc;
using Xbim.Ifc4.GeometryResource;
using Xbim.Ifc4.GeometricModelResource;
using Xbim.Ifc4.RepresentationResource;
using Xbim.Ifc4.SharedBldgElements;
using Xbim.Ifc4.GeometricConstraintResource;
using Xbim.Ifc4.MaterialResource;
using Xbim.Ifc4.ProductExtension;
using Xbim.Ifc4.PresentationOrganizationResource;
using Bim.Domain;
using Bim.Domain.Ifc;

namespace Bim.Application.Ifc
{
    public class IfSill
    {
        #region Properties
        public int Id { get; set; }
        public string Label { get; set; }
        public IDimension dimension { get; set; }
        public ILocation location { get; set; }
        public IfWall wall { get; set; }
        public IfModel IfModel { get; set; }
        public IfcBeamStandardCase IfcBeam { get; set; }
        public IfcProductDefinitionShape Representation { get; set; }

        #endregion


        #region Member Variables

        private IfcStore IfcModel;
        #endregion
        #region Constructor
        public IfSill(IfModel ifModel, float x, float y, float z, float xDim, float yDim, float ZDim, string label)
        {
            Label = label;
            IfModel = ifModel;
            IfcModel = ifModel.IfcStore;
            dimension = new IfDimension(xDim, yDim, ZDim);
            location = new IfLocation(x, y, z);
            using (var txn=IfcModel.BeginTransaction("NewBeam"))
            {
               
               
            }
            
            New();
            //  New(name);

        }
        public IfSill(IfModel model, IfLocation location, IfDimension dimension, string label) : this(model, location.X, location.Y, location.Z, dimension.XDim, dimension.YDim, dimension.ZDim, label)
        {

        }

        #endregion
        #region Methods
        //public void New()
        //{
        //    using (var txn = IfcModel.BeginTransaction("New Beam"))
        //    {
        //        var sill = IfcModel.Instances.New<IfcBeamStandardCase>();
        //        sill.Name = "sill-test";

        //        var recProfile = IfcModel.Instances.New<IfcRectangleProfileDef>();
        //        //filling proerties eshta y3ny
        //        recProfile.ProfileType = IfcProfileTypeEnum.AREA;
        //        recProfile.XDim = dimension.XDim;
        //        recProfile.YDim = dimension.YDim;

        //        var body = IfcModel.Instances.New<IfcExtrudedAreaSolid>();
        //        body.Depth = dimension.ZDim;
        //        //rectangle profile
        //        body.SweptArea = recProfile;
        //        body.ExtrudedDirection = IfcModel.Instances.New<IfcDirection>();
        //        body.ExtrudedDirection.SetXYZ(0, 0, 1);
        //        //parameters to insert the geometry in the model
        //        var origin = IfcModel.Instances.New<IfcCartesianPoint>();

        //        /*          Set Stud Location */
        //        origin.SetXYZ(location.X, location.Y, location.Z);

        //        var point = IfcModel.Instances.New<IfcCartesianPoint>();
        //        point.SetXYZ(0, 0, 0);

        //        body.Position = IfcModel.Instances.New<IfcAxis2Placement3D>();
        //        body.Position.Location = point;

        //        //Create a Definition shape to hold the geometry
        //        var shape = IfcModel.Instances.New<IfcShapeRepresentation>();
        //        var modelContext = IfcModel.Instances.OfType<IfcGeometricRepresentationContext>().FirstOrDefault();
        //        shape.ContextOfItems = modelContext;
        //        shape.RepresentationType = "SweptSolid";
        //        shape.RepresentationIdentifier = "Body";
        //        shape.Items.Add(body);

        //        var rep = IfcModel.Instances.New<IfcProductDefinitionShape>();
        //        rep.Representations.Add(shape);
        //        sill.Representation = rep;

        //        #region Placement

        //        var lp = IfcModel.Instances.New<IfcLocalPlacement>();
        //        var ax3D = IfcModel.Instances.New<IfcAxis2Placement3D>();
        //        /*          Set Stud Location */

        //        ax3D.Location = origin;
        //        ax3D.RefDirection = IfcModel.Instances.New<IfcDirection>();
        //        ax3D.RefDirection.SetXYZ(1, 0, 0); //x-axis direction
        //        ax3D.Axis = IfcModel.Instances.New<IfcDirection>();
        //        ax3D.Axis.SetXYZ(0, 0, 1); //z-axis direction
        //        lp.RelativePlacement = ax3D;
        //        sill.ObjectPlacement = lp;

        //        // linear segment as IfcPolyline with two points is required for IfcWall

        //        var ifcPolyline = IfcModel.Instances.New<IfcPolyline>();
        //        var startPoint = IfcModel.Instances.New<IfcCartesianPoint>();
        //        startPoint.SetXY(location.X, location.Y);
        //        var endPoint = IfcModel.Instances.New<IfcCartesianPoint>();
        //        /*          Set Stud Location */
        //        endPoint.SetXY(location.X + dimension.XDim, location.Y + dimension.YDim);
        //        ifcPolyline.Points.Add(startPoint);
        //        ifcPolyline.Points.Add(endPoint);

        //        var shape2D = IfcModel.Instances.New<IfcShapeRepresentation>();
        //        shape2D.ContextOfItems = modelContext;
        //        shape2D.RepresentationIdentifier = "Axis";
        //        shape2D.RepresentationType = "Curve2D";
        //        shape2D.Items.Add(ifcPolyline);
        //        rep.Representations.Add(shape2D);

        //        #endregion



        //        #region Material
        //        // Where Clause: The IfcWallStandard relies on the provision of an IfcMaterialLayerSetUsage 
        //        var ifcMaterialLayerSetUsage = IfcModel.Instances.New<IfcMaterialLayerSetUsage>();
        //        var ifcMaterialLayerSet = IfcModel.Instances.New<IfcMaterialLayerSet>();
        //        var ifcMaterialLayer = IfcModel.Instances.New<IfcMaterialLayer>();

        //        ifcMaterialLayer.LayerThickness = dimension.YDim;
        //        ifcMaterialLayerSet.MaterialLayers.Add(ifcMaterialLayer);
        //        ifcMaterialLayerSetUsage.ForLayerSet = ifcMaterialLayerSet;
        //        ifcMaterialLayerSetUsage.LayerSetDirection = IfcLayerSetDirectionEnum.AXIS2;
        //        ifcMaterialLayerSetUsage.DirectionSense = IfcDirectionSenseEnum.POSITIVE;
        //        ifcMaterialLayerSetUsage.OffsetFromReferenceLine = 10;
        //        // Add material to wall



        //        var material = IfcModel.Instances.New<IfcMaterial>();
        //        material.Name = "some material";
        //        var ifcRelAssociatesMaterial = IfcModel.Instances.New<IfcRelAssociatesMaterial>();
        //        ifcRelAssociatesMaterial.RelatingMaterial = material;
        //        ifcRelAssociatesMaterial.RelatedObjects.Add(sill);

        //        ifcRelAssociatesMaterial.RelatingMaterial = ifcMaterialLayerSetUsage;

        //        // IfcPresentationLayerAssignment is required for CAD presentation in IfcWall or IfcWallStandardCase
        //        var ifcPresentationLayerAssignment = IfcModel.Instances.New<IfcPresentationLayerAssignment>();
        //        ifcPresentationLayerAssignment.Name = "some ifcPresentationLayerAssignment";
        //        ifcPresentationLayerAssignment.AssignedItems.Add(shape);
        //        //we need to give the stud a building.
        //        var building = (IfcBuilding)IfcModel.Instances.OfType<IIfcBuilding>().FirstOrDefault();
        //        building.AddElement(sill);
        //        #endregion
        //        txn.Commit();

        //    }
        //}
        public IfcBeamStandardCase New()
        {
            using (var txn = IfcModel.BeginTransaction("New Beam"))
            {
                IfcBeam = IfcModel.Instances.New<IfcBeamStandardCase>();
                SetShape();
                SetLocation();
                SetMaterial("sill-Material");
                //now place the wall into the model

                var building = (IfcBuilding)IfcModel.Instances.OfType<IIfcBuilding>().FirstOrDefault();
                building.AddElement(IfcBeam);
                txn.Commit();
            }
            return IfcBeam;

        }

        #endregion

        #region Helper Method
        public void SetShape()
        {
            var recProfile = IfcModel.Instances.New<IfcRectangleProfileDef>();
            //filling proerties eshta y3ny
            recProfile.ProfileType = IfcProfileTypeEnum.AREA;
            recProfile.XDim = dimension.XDim;
            recProfile.YDim = dimension.YDim;

            var body = IfcModel.Instances.New<IfcExtrudedAreaSolid>();
            body.Depth = dimension.ZDim;
            //rectangle profile
            body.SweptArea = recProfile;
            body.ExtrudedDirection = IfcModel.Instances.New<IfcDirection>();
            body.ExtrudedDirection.SetXYZ(0, 0, 1);
            //parameters to insert the geometry in the model
            var origin = IfcModel.Instances.New<IfcCartesianPoint>();

            /*          Set Stud Location */
            origin.SetXYZ(location.X, location.Y, location.Z);

            var point = IfcModel.Instances.New<IfcCartesianPoint>();
            point.SetXYZ(0, 0, 0);

            body.Position = IfcModel.Instances.New<IfcAxis2Placement3D>();
            body.Position.Location = point;

            //Create a Definition shape to hold the geometry
            var shape = IfcModel.Instances.New<IfcShapeRepresentation>();
            var modelContext = IfcModel.Instances.OfType<IfcGeometricRepresentationContext>().FirstOrDefault();
            shape.ContextOfItems = modelContext;
            shape.RepresentationType = "SweptSolid";
            shape.RepresentationIdentifier = "Body";
            shape.Items.Add(body);

            var rep = IfcModel.Instances.New<IfcProductDefinitionShape>();
            rep.Representations.Add(shape);

            // linear segment as IfcPolyline with two points is required for IfcWall

            var ifcPolyline = IfcModel.Instances.New<IfcPolyline>();
            var startPoint = IfcModel.Instances.New<IfcCartesianPoint>();
            startPoint.SetXY(location.X, location.Y);
            var endPoint = IfcModel.Instances.New<IfcCartesianPoint>();
            /*          Set Stud Location */
            endPoint.SetXY(location.X + dimension.XDim, location.Y + dimension.YDim);
            ifcPolyline.Points.Add(startPoint);
            ifcPolyline.Points.Add(endPoint);

            var shape2D = IfcModel.Instances.New<IfcShapeRepresentation>();
            shape2D.ContextOfItems = modelContext;
            shape2D.RepresentationIdentifier = "Axis";
            shape2D.RepresentationType = "Curve2D";
            shape2D.Items.Add(ifcPolyline);
            rep.Representations.Add(shape2D);
            IfcBeam.Representation = rep;
        }
        public void SetLocation()
        {
            //parameters to insert the geometry in the model
            var origin = IfcModel.Instances.New<IfcCartesianPoint>();

            /*          Set Stud Location */
            origin.SetXYZ(location.X, location.Y, location.Z);

            var lp = IfcModel.Instances.New<IfcLocalPlacement>();
            var ax3D = IfcModel.Instances.New<IfcAxis2Placement3D>();
            /*          Set Stud Location */

            ax3D.Location = origin;
            ax3D.RefDirection = IfcModel.Instances.New<IfcDirection>();
            ax3D.RefDirection.SetXYZ(1, 0, 0); //x-axis direction
            ax3D.Axis = IfcModel.Instances.New<IfcDirection>();
            ax3D.Axis.SetXYZ(0, 0, 1); //z-axis direction
            lp.RelativePlacement = ax3D;

        }
        public void SetMaterial(string name)
        {
            var ifcMaterialLayerSetUsage = IfcModel.Instances.New<IfcMaterialLayerSetUsage>();
            var ifcMaterialLayerSet = IfcModel.Instances.New<IfcMaterialLayerSet>();
            var ifcMaterialLayer = IfcModel.Instances.New<IfcMaterialLayer>();

            ifcMaterialLayer.LayerThickness = dimension.YDim;
            ifcMaterialLayerSet.MaterialLayers.Add(ifcMaterialLayer);
            ifcMaterialLayerSetUsage.ForLayerSet = ifcMaterialLayerSet;
            ifcMaterialLayerSetUsage.LayerSetDirection = IfcLayerSetDirectionEnum.AXIS2;
            ifcMaterialLayerSetUsage.DirectionSense = IfcDirectionSenseEnum.POSITIVE;
            ifcMaterialLayerSetUsage.OffsetFromReferenceLine = 10;
            // Add material to wall


            var material = IfcModel.Instances.New<IfcMaterial>();
            material.Name = name;
            var ifcRelAssociatesMaterial = IfcModel.Instances.New<IfcRelAssociatesMaterial>();
            ifcRelAssociatesMaterial.RelatingMaterial = material;

            //attach material to specific element
            ifcRelAssociatesMaterial.RelatedObjects.Add(IfcBeam);

            ifcRelAssociatesMaterial.RelatingMaterial = ifcMaterialLayerSetUsage;

            // IfcPresentationLayerAssignment is required for CAD presentation in IfcWall or IfcWallStandardCase
            var ifcPresentationLayerAssignment = IfcModel.Instances.New<IfcPresentationLayerAssignment>();

            ifcPresentationLayerAssignment.Name = "some ifcPresentationLayerAssignment";
            var shape = IfcBeam.Representation.Representations.FirstOrDefault();            //get ifc element shape
            ifcPresentationLayerAssignment.AssignedItems.Add(shape);

        }

        #endregion
    }
}
