using Bim.Common;
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

using System.Diagnostics;
using Xbim.Common;
using Xbim.Common.Step21;
using Xbim.IO;
using Xbim.Ifc4.ActorResource;
using Xbim.Ifc4.DateTimeResource;
using Xbim.Ifc4.ExternalReferenceResource;
using Xbim.Ifc4.Kernel;
using Xbim.Ifc4.MeasureResource;
using Xbim.Ifc4.PropertyResource;
using Xbim.Ifc4.QuantityResource;

namespace Bim.Components
{
   public class Sill
    {
        #region Properties


        public string Label { get; set; }

        public Dimension dimension { get; set; }
        public Location location { get; set; }
        public Wall wall { get; set; }
        public Model Model { get; set; }
        public IfcBeamStandardCase sill { get; set; }
        #endregion


        #region Member Variables

        private IfcStore model;
        #endregion

        #region Constructor

        public Sill(Model model, float x, float y, float z, float xDim, float yDim, float height, string name)
        {
            Model = model;
            this.model = model.IfcModel;
            dimension = new Dimension(xDim, yDim, height);
            location = new Location(x, y, z);
            New(name);

        }
        public Sill(Model model, Location location, Dimension dimension, string name) : this(model, location.X, location.Y, location.Z, dimension.XDIM, dimension.YDIM, dimension.Height, name)
        {

        }

        #endregion



        #region Methods
        public IfcBeamStandardCase New(string name)
        {
            using (var txn = model.BeginTransaction("New Beam"))
            {
                var sill = model.Instances.New<IfcBeamStandardCase>();
                sill.Name = name;
                //new recprofile
                var recProfile = model.Instances.New<IfcRectangleProfileDef>();
                //filling proerties eshta y3ny
                recProfile.ProfileType = IfcProfileTypeEnum.AREA;
                recProfile.XDim = dimension.XDIM;
                recProfile.YDim = dimension.YDIM;

                //new cartesian point
                var insertPoint = model.Instances.New<IfcCartesianPoint>();
                insertPoint.SetXY(0, 0); //we have to change this.
                                         //
                recProfile.Position = model.Instances.New<IfcAxis2Placement2D>();
                recProfile.Position.Location = insertPoint;

                //model as a swept area solid
                var body = model.Instances.New<IfcExtrudedAreaSolid>();
                body.Depth = dimension.Height;
                //rectangle profile
                body.SweptArea = recProfile;
                body.ExtrudedDirection = model.Instances.New<IfcDirection>();
                body.ExtrudedDirection.SetXYZ(0, 0, 1);
                //parameters to insert the geometry in the model
                var origin = model.Instances.New<IfcCartesianPoint>();

                /*          Set Stud Location */
                origin.SetXYZ(location.X, location.Y, location.Z);

                body.Position = model.Instances.New<IfcAxis2Placement3D>();
                body.Position.Location = origin;

                //Create a Definition shape to hold the geometry
                var shape = model.Instances.New<IfcShapeRepresentation>();
                var modelContext = model.Instances.OfType<IfcGeometricRepresentationContext>().FirstOrDefault();
                shape.ContextOfItems = modelContext;
                shape.RepresentationType = "SweptSolid";
                shape.RepresentationIdentifier = "Body";
                shape.Items.Add(body);


                var rep = model.Instances.New<IfcProductDefinitionShape>();
                rep.Representations.Add(shape);
                sill.Representation = rep;

                //now place the wall into the model

                #region Placement

                var lp = model.Instances.New<IfcLocalPlacement>();
                var ax3D = model.Instances.New<IfcAxis2Placement3D>();
                /*          Set Stud Location */
                var point = model.Instances.New<IfcCartesianPoint>();
                point.SetXYZ(0, 0, 0);
                ax3D.Location = point;
                ax3D.RefDirection = model.Instances.New<IfcDirection>();
                ax3D.RefDirection.SetXYZ(1, 0, 0);
                ax3D.Axis = model.Instances.New<IfcDirection>();
                ax3D.Axis.SetXYZ(0, 1, 0);
                lp.RelativePlacement = ax3D;
                sill.ObjectPlacement = lp;

                // linear segment as IfcPolyline with two points is required for IfcWall
                var ifcPolyline = model.Instances.New<IfcPolyline>();
                var startPoint = model.Instances.New<IfcCartesianPoint>();
                startPoint.SetXY(location.X, location.Y);
                var endPoint = model.Instances.New<IfcCartesianPoint>();

                /*          Set Stud Location */
                endPoint.SetXY(location.X+dimension.XDIM, location.Y+dimension.YDIM);
                ifcPolyline.Points.Add(startPoint);
                ifcPolyline.Points.Add(endPoint);

                var shape2D = model.Instances.New<IfcShapeRepresentation>();
                shape2D.ContextOfItems = modelContext;
                shape2D.RepresentationIdentifier = "Axis";
                shape2D.RepresentationType = "Curve2D";
                shape2D.Items.Add(ifcPolyline);
                rep.Representations.Add(shape2D);
                #endregion



                #region Material
                // Where Clause: The IfcWallStandard relies on the provision of an IfcMaterialLayerSetUsage 
                var ifcMaterialLayerSetUsage = model.Instances.New<IfcMaterialLayerSetUsage>();
                var ifcMaterialLayerSet = model.Instances.New<IfcMaterialLayerSet>();
                var ifcMaterialLayer = model.Instances.New<IfcMaterialLayer>();

                ifcMaterialLayer.LayerThickness = 10;
                ifcMaterialLayerSet.MaterialLayers.Add(ifcMaterialLayer);
                ifcMaterialLayerSetUsage.ForLayerSet = ifcMaterialLayerSet;
                ifcMaterialLayerSetUsage.LayerSetDirection = IfcLayerSetDirectionEnum.AXIS2;
                ifcMaterialLayerSetUsage.DirectionSense = IfcDirectionSenseEnum.POSITIVE;
                ifcMaterialLayerSetUsage.OffsetFromReferenceLine = 10;

                // Add material to wall



                var material = model.Instances.New<IfcMaterial>();
                material.Name = "some material";
                var ifcRelAssociatesMaterial = model.Instances.New<IfcRelAssociatesMaterial>();
                ifcRelAssociatesMaterial.RelatingMaterial = material;
                ifcRelAssociatesMaterial.RelatedObjects.Add(sill);

                ifcRelAssociatesMaterial.RelatingMaterial = ifcMaterialLayerSetUsage;

                // IfcPresentationLayerAssignment is required for CAD presentation in IfcWall or IfcWallStandardCase
                var ifcPresentationLayerAssignment = model.Instances.New<IfcPresentationLayerAssignment>();
                ifcPresentationLayerAssignment.Name = "some ifcPresentationLayerAssignment";
                ifcPresentationLayerAssignment.AssignedItems.Add(shape);

                //we need to give the stud a building.
                var building = (IfcBuilding)model.Instances.OfType<IIfcBuilding>().FirstOrDefault();
                building.AddElement(sill);
                #endregion
                txn.Commit();



                return this.sill = sill;

            }

        }
        #endregion

        #region Helper Method


        #endregion
    }
}
