using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xbim.Ifc4.Interfaces;
using Xbim.Ifc4.ProfileResource;
using Xbim.Ifc;
using Xbim.Ifc4.GeometryResource;

using Xbim.Ifc4.RepresentationResource;
using Xbim.Ifc4.SharedBldgElements;
using Xbim.Ifc4.GeometricConstraintResource;
using Xbim.Ifc4.MaterialResource;
using Xbim.Ifc4.ProductExtension;
using Xbim.Ifc4.PresentationOrganizationResource;


using Xbim.Ifc4.GeometricModelResource;
using Bim.Domain;
using Bim.Domain.Ifc;

namespace Bim.Application.Ifc
{
    public class IfStud
    {
        #region Properties

        public string Label { get; set; }
        public IfDimension dimension { get; set; }
        public IfLocation location { get; set; }
        public IfWall wall { get; set; }
        public IfcColumnStandardCase stud { get; set; }
        public IfcAxis2Placement3D RelativeAxis { get; set; }
        public IfModel Model { get; set; }

        #endregion

        #region Member Variables
        private IfcStore ifcModel;

        #endregion


        #region Constructor

        public IfStud(IfModel model,IfWall wall, float x, float y, float z, float xDim, float yDim, float ZDim, string name)
        {
            Model = model;
            this.wall = wall;
            ifcModel = wall.IfcStore;
            dimension = new IfDimension(xDim, yDim, ZDim);
            location = new IfLocation(x, y, z);
            RelativeAxis = (IfcAxis2Placement3D)wall.WallAxis;
            New(name, RelativeAxis);
        }
        public IfStud(IfModel model,IfWall wall, IfLocation location, IfDimension dimension, string name) : this(model,wall, location.X, location.Y, location.Z, dimension.XDim, dimension.YDim, dimension.ZDim, name)
        {
        }

        #endregion

        #region Methods

        public IfcColumnStandardCase New(string name, IfcAxis2Placement3D relativeAxis)
        {


            using (var txn = ifcModel.BeginTransaction("New Stud"))
            {
                var stud = ifcModel.Instances.New<IfcColumnStandardCase>();
                /*      Set Wall Name             */
                stud.Name = name;

                /*      new recprofile             */

                var recProfile = ifcModel.Instances.New<IfcRectangleProfileDef>();
                recProfile.ProfileType = IfcProfileTypeEnum.AREA;
                recProfile.XDim = dimension.XDim;
                recProfile.YDim = dimension.YDim;

                /*      new cartesian point             */

                var insertPoint = ifcModel.Instances.New<IfcCartesianPoint>();
                insertPoint.SetXY(0, 0);

                recProfile.Position = ifcModel.Instances.New<IfcAxis2Placement2D>();
                recProfile.Position.Location = insertPoint;

                //ifcModel as a swept area solid
                var body = ifcModel.Instances.New<IfcExtrudedAreaSolid>();
                body.Depth = dimension.ZDim;
                //rectangle profile
                body.SweptArea = recProfile;
                body.ExtrudedDirection = ifcModel.Instances.New<IfcDirection>();
                body.ExtrudedDirection.SetXYZ(0, 0, 1); //z-axis dierection
                                                        //parameters to insert the geometry in the ifcModel

                /*          Set Stud Location */
                var origin = ifcModel.Instances.New<IfcCartesianPoint>();

                origin.SetXYZ(location.X, location.Y, location.Z);

                var point = ifcModel.Instances.New<IfcCartesianPoint>();
                point.SetXYZ(0, 0, 0);

                body.Position = ifcModel.Instances.New<IfcAxis2Placement3D>();
                body.Position.Location = point;

                //Create a Definition shape to hold the geometry
                var shape = ifcModel.Instances.New<IfcShapeRepresentation>();
                var ifcModelContext = ifcModel.Instances.OfType<IfcGeometricRepresentationContext>().FirstOrDefault();
                shape.ContextOfItems = ifcModelContext;
                shape.RepresentationType = "SweptSolid";
                shape.RepresentationIdentifier = "Body";
                shape.Items.Add(body);


                var rep = ifcModel.Instances.New<IfcProductDefinitionShape>();
                rep.Representations.Add(shape);
                stud.Representation = rep;

                //now place the wall into the ifcModel

                #region Placement

                var lp = ifcModel.Instances.New<IfcLocalPlacement>();
                var ax3D = ifcModel.Instances.New<IfcAxis2Placement3D>();
                lp.PlacementRelTo = (IfcLocalPlacement)wall.localPlacement;
                /*          Set Stud Location */
                ax3D.Location = origin;
                ax3D.RefDirection = ifcModel.Instances.New<IfcDirection>();

                ax3D.RefDirection.SetXYZ(1, 0, 0);//x-axis direction
                ax3D.Axis = ifcModel.Instances.New<IfcDirection>();
                ax3D.Axis.SetXYZ(0, 0, 1); //z-axis direction
                /***         Set Stud Relative Axis  ***/
                if (relativeAxis==null)
                {
                    lp.RelativePlacement = ax3D;
                }
                else
                {
                    lp.RelativePlacement = ax3D;
                  //  relativeAxis.Location =  origin;
                }
                
                stud.ObjectPlacement = lp;

                // linear segment as IfcPolyline with two points is required for IfcWall

                /***         Set Stud 2D coordinations  ***/
                var ifcPolyline = ifcModel.Instances.New<IfcPolyline>();
                var startPoint = ifcModel.Instances.New<IfcCartesianPoint>();
                startPoint.SetXY(origin.X, origin.Y);
                var endPoint = ifcModel.Instances.New<IfcCartesianPoint>();

                /*          Set Stud Location */
                endPoint.SetXY(origin.X + dimension.XDim, origin.Y + dimension.YDim);
                ifcPolyline.Points.Add(startPoint);
                ifcPolyline.Points.Add(endPoint);

                var shape2D = ifcModel.Instances.New<IfcShapeRepresentation>();
                shape2D.ContextOfItems = ifcModelContext;
                shape2D.RepresentationIdentifier = "Axis";
                shape2D.RepresentationType = "Curve2D";
                shape2D.Items.Add(ifcPolyline);
                rep.Representations.Add(shape2D);
                #endregion



                #region Material
                // Where Clause: The IfcWallStandard relies on the provision of an IfcMaterialLayerSetUsage 
                var ifcMaterialLayerSetUsage = ifcModel.Instances.New<IfcMaterialLayerSetUsage>();
                var ifcMaterialLayerSet = ifcModel.Instances.New<IfcMaterialLayerSet>();
                var ifcMaterialLayer = ifcModel.Instances.New<IfcMaterialLayer>();

                ifcMaterialLayer.LayerThickness = 20;
                ifcMaterialLayerSet.MaterialLayers.Add(ifcMaterialLayer);
                ifcMaterialLayerSetUsage.ForLayerSet = ifcMaterialLayerSet;
                ifcMaterialLayerSetUsage.LayerSetDirection = IfcLayerSetDirectionEnum.AXIS2;
                ifcMaterialLayerSetUsage.DirectionSense = IfcDirectionSenseEnum.POSITIVE;
                ifcMaterialLayerSetUsage.OffsetFromReferenceLine = 150;

                // Add material to wall



                var material = ifcModel.Instances.New<IfcMaterial>();
                material.Name = "some material";
                var ifcRelAssociatesMaterial = ifcModel.Instances.New<IfcRelAssociatesMaterial>();
                ifcRelAssociatesMaterial.RelatingMaterial = material;
                ifcRelAssociatesMaterial.RelatedObjects.Add(stud);

                ifcRelAssociatesMaterial.RelatingMaterial = ifcMaterialLayerSetUsage;

                // IfcPresentationLayerAssignment is required for CAD presentation in IfcWall or IfcWallStandardCase
                var ifcPresentationLayerAssignment = ifcModel.Instances.New<IfcPresentationLayerAssignment>();
                ifcPresentationLayerAssignment.Name = "some ifcPresentationLayerAssignment";
                ifcPresentationLayerAssignment.AssignedItems.Add(shape);

                //we need to give the stud a building.
                var building = (IfcBuilding)ifcModel.Instances.OfType<IIfcBuilding>().FirstOrDefault();
                building.AddElement(stud);
                #endregion
                txn.Commit();



                return this.stud = stud;

            }




            #endregion




            #region Helper Method


            #endregion
        }
    }
}