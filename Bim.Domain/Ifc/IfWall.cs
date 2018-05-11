using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xbim.Ifc;
using Xbim.Ifc.ViewModels;
using Xbim.Ifc4.GeometricConstraintResource;
using Xbim.Ifc4.Interfaces;
using Xbim.Ifc4.MeasureResource;
using Bim.Domain;

namespace Bim.Domain.Ifc
{
    /// <summary>
    /// Wall object to Carry IFCStandardWall Data
    /// </summary>

    public class IfWall : IWall
    {
        public static IEnumerable<IIfcWallStandardCase> IfcWalls { get; set; }

        #region wall Object Properties


        public ILocation Location { get; set; }
        public List<IDoor> Doors { get; set; }
        public List<IWindow> Windows { get; set; }
        public IDimension Dimensions { get; set; }
        public int Label { get; set; }
        public bool IsExternal { get; set; }
        #endregion
        public IfcStore IfcStore { get; set; }
        public static IfcStore IfcModel { get; set; }
        public IIfcAxis2Placement3D WallAxis { get; set; }
        public int Id { get; set; }
        string IElement.Label { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IIfcLocalPlacement localPlacement;
        #region Constructors


        public IfWall(IfcStore iifcModel, float xDim, float yDim, float ZDim, float x, float y, float z)
        {
            IfcModel = iifcModel;
            IfcStore = iifcModel;
            Location = new IfLocation(x, y, z);
            Dimensions = new IfDimension(xDim, yDim, ZDim);
            Doors = new List<IDoor>();
            Windows = new List<IWindow>();

        }

        public IfWall(IfcStore ifcModel, IfDimension dimensions, IfLocation l) : this(ifcModel, dimensions.XDim, dimensions.YDim, dimensions.ZDim, l.X, l.Y, l.Z)
        {


        }

        public IfWall(IfLocation location) : this(null, 0, 0, 0, location.X, location.Y, location.Z)
        {

        }
        public IfWall(IfDimension dimensions) : this(null, dimensions.XDim, dimensions.YDim, dimensions.ZDim, 0, 0, 0)
        {

        }
        #endregion

        #region  static Methods



        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static IEnumerable<IIfcWallStandardCase> GetIfcWalls(IfModel model)
        {
            return IfcWalls = model.IfcStore.Instances.OfType<IIfcWallStandardCase>();
        }

        /// <summary>
        ///Get Wall List from the static IfcWalls Collection
        /// </summary>
        /// <returns></returns>
        public static List<IfWall> ExtractWalls(IfModel model)
        {
            List<IfWall> wallsList = new List<IfWall>();
            IfcWalls = model.IfcStore.Instances.OfType<IIfcWallStandardCase>();
            foreach (var wall in IfcWalls)
            {
                var recD =  wall.Representation.Representations
                                .SelectMany(a => a.Items)
                                .OfType<IIfcExtrudedAreaSolid>().Select(a => a.SweptArea)
                                .OfType<IIfcRectangleProfileDef>().FirstOrDefault() ??
                            wall.Representation.Representations
                                .SelectMany(a => a.Items)
                                .OfType<IIfcBooleanClippingResult>().Select(a => a.FirstOperand)
                                .OfType<IIfcExtrudedAreaSolid>().Select(a => a.SweptArea)
                                .OfType<IIfcRectangleProfileDef>().FirstOrDefault();

                //get the wall x,y,z
                if (recD != null)
                {
                    IfWall crntWall = new IfWall(new IfDimension())
                    {
                        Label = wall.EntityLabel,
                        IfcStore = model.IfcStore
                    };
                    GetLocation(wall, crntWall);
                    GetDimension(wall, crntWall);
                    //Extract Openings
                    GetOpenings(wall, crntWall);
                    //get wall label
                    //check if Wall is External
                    CheckExternal(wall, crntWall);
                    wallsList.Add(crntWall);
                }
            }

            return wallsList;
        }
        #endregion

        #region Helper Private Functions


        private static void CheckExternal(IIfcWallStandardCase wall, IfWall crntWall)
        {
            var res = (bool)wall.IsDefinedBy
                    .Where(r => r.RelatingPropertyDefinition is IIfcPropertySet)
                    .SelectMany(r => ((IIfcPropertySet)r.RelatingPropertyDefinition).HasProperties)
                    .OfType<IIfcPropertySingleValue>().Where(a => a.Name == "IsExternal").Select(a => a.NominalValue).FirstOrDefault().Value;

            if (res == true)
            {
                crntWall.IsExternal = true;
            }
            else
            {
                crntWall.IsExternal = false;
            }

        }
        private static void GetOpenings(IIfcWallStandardCase wall, IfWall crntWall)
        {
            foreach (var opening in wall.HasOpenings)
            {
                var opnng = (IIfcAxis2Placement3D)((IIfcLocalPlacement)opening.RelatedOpeningElement.ObjectPlacement).RelativePlacement;
                var oLocation = opnng.Location;
                var recProfile = opening.RelatedOpeningElement.Representation.Representations.SelectMany(a => a.Items).OfType<IIfcExtrudedAreaSolid>().Select(a => a.SweptArea).OfType<IIfcRectangleProfileDef>().FirstOrDefault();
                var recDepth = opening.RelatedOpeningElement.Representation.Representations.SelectMany(a => a.Items).OfType<IIfcExtrudedAreaSolid>().Select(a => a.Depth);
                var filling = ((IIfcOpeningElement)opening.RelatedOpeningElement).HasFillings.FirstOrDefault().RelatedBuildingElement.GetType().Name;


                if (filling == "IfcDoor")
                {
                    crntWall.Doors.Add(new IfDoor((float)recProfile.XDim, (float)recDepth.FirstOrDefault(), (float)recProfile.YDim
                        , (float)oLocation.X, (float)oLocation.Y, (float)oLocation.Z));
                }
                else
                {

                    crntWall.Windows.Add(new IfWindow((float)recDepth.FirstOrDefault(), (float)recProfile.YDim, (float)recProfile.XDim
                        , (float)oLocation.X, (float)oLocation.Y, (float)oLocation.Z));

                }
            }

        }
        private static void GetLocation(IIfcWallStandardCase wall, IfWall crntWall)
        {
            //get the wall x,y,z
            // var recD = wall.Representation.Representations.SelectMany(a => a.Items).OfType<IIfcExtrudedAreaSolid>().Select(a => a.SweptArea).OfType<IIfcRectangleProfileDef>().FirstOrDefault();
            //get the wall thickness
            //  var thickness = wall.HasAssociations.OfType<IIfcRelAssociatesMaterial>().OfType<IIfcMaterialLayerSetUsage>().Select(a => a.OffsetFromReferenceLine);//.OfType<IfcPositiveLengthMeasure>();
            crntWall.WallAxis = ((IIfcAxis2Placement3D)((IIfcLocalPlacement)wall.ObjectPlacement).RelativePlacement);
            var location = crntWall.WallAxis.Location;
            crntWall.localPlacement = (IIfcLocalPlacement)wall.ObjectPlacement;
            //using the Wall Class;
            crntWall.Location = new IfLocation((float)location.X, (float)location.Y, (float)location.Z);
        }
        private static void GetDimension(IIfcWallStandardCase wall, IfWall crntWall)
        {
            //get the wall x,y,z
            var recD = wall.Representation.Representations
                            .SelectMany(a => a.Items)
                            .OfType<IIfcExtrudedAreaSolid>().Select(a => a.SweptArea)
                            .OfType<IIfcRectangleProfileDef>().FirstOrDefault() ?? 
                       wall.Representation.Representations
                            .SelectMany(a => a.Items)
                            .OfType<IIfcBooleanClippingResult>().Select(a => a.FirstOperand)
                            .OfType<IIfcExtrudedAreaSolid>().Select(a => a.SweptArea)
                            .OfType<IIfcRectangleProfileDef>().FirstOrDefault();

            var depth = wall.Representation.Representations
                            .SelectMany(a => a.Items)
                            .OfType<IIfcExtrudedAreaSolid>().Select(a => a.Depth) ?? 
                        wall.Representation.Representations
                            .SelectMany(a => a.Items)
                            .OfType<IIfcBooleanClippingResult>().Select(a => a.FirstOperand)
                            .OfType<IIfcExtrudedAreaSolid>().Select(a => a.Depth);


            //var recD = wall.Representation.Representations.SelectMany(a => a.Items).OfType<IIfcExtrudedAreaSolid>().Select(a => a.SweptArea).OfType<IIfcRectangleProfileDef>().FirstOrDefault();
            //var recD1 = wall.Representation.Representations.SelectMany(a => a.Items).OfType<IIfcBooleanClippingResult>().Select(a => a.FirstOperand).OfType<IIfcExtrudedAreaSolid>().Select(a => a.SweptArea).OfType<IIfcRectangleProfileDef>().FirstOrDefault();
            //var otherDepth = wall.Representation.Representations.SelectMany(a => a.Items).OfType<IIfcBooleanClippingResult>().Select(a => a.FirstOperand).OfType<IIfcExtrudedAreaSolid>().Select(a => a.Depth);
            //var depth = wall.Representation.Representations.SelectMany(a => a.Items).OfType<IIfcExtrudedAreaSolid>().Select(a => a.Depth);
            //get the wall thickness
            var thickness = wall.HasAssociations.OfType<IIfcRelAssociatesMaterial>().OfType<IIfcMaterialLayerSetUsage>().Select(a => a.OffsetFromReferenceLine);//.OfType<IfcPositiveLengthMeasure>();
            var location = ((IIfcAxis2Placement3D)((IIfcLocalPlacement)wall.ObjectPlacement).RelativePlacement).Location;
            //using the Wall Class;
            if (recD !=null && depth != null)
            {
                crntWall.Dimensions = new IfDimension((float)recD.XDim, (float)recD.YDim, (float)depth.FirstOrDefault());
            }
        }
        #endregion



    }

}

