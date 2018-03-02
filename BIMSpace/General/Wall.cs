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
using Xbim.Common;

namespace BIMSpace.General
{
    /// <summary>
    /// Wall object to Carry IFCStandardWall Data
    /// </summary>

    public class Wall
    {
        public static IEnumerable<IIfcWallStandardCase> IfcWalls { get; set; }

        #region wall Object Properties


        public Location Location { get; set; }
        public List<Door> Doors { get; set; }
        public List<Window> windows { get; set; }
        public Dimension Dimensions { get; set; }
        public int Label { get; set; }
        public bool IsExternal { get; set; }
        #endregion

        #region Constructors


        public Wall(float xDim, float yDim, float height, float x, float y, float z)
        {
            Location = new Location(x, y, z);
            Dimensions = new Dimension(xDim, yDim, height);
            Doors = new List<Door>();
            windows = new List<Window>();


        }

        public Wall(Dimension dimensions, Location l) : this(dimensions.XDIM, dimensions.YDIM, dimensions.Height, l.X, l.Y, l.Z)
        {


        }

        public Wall(Location location) : this(0, 0, 0, location.X, location.Y, location.Z)
        {

        }
        public Wall(Dimension dimensions) : this(dimensions.XDIM, dimensions.YDIM, dimensions.Height, 0, 0, 0)
        {

        }
        #endregion

        #region  static Methods

        /// <summary>
        /// obtain Collection of Ifcwallstandard Case from an Ifc File
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static IEnumerable<IIfcWallStandardCase> GetWalls(string filePath)
        {
            var model = IfcStore.Open(filePath);
            return IfcWalls = model.Instances.OfType<IIfcWallStandardCase>();


        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static IEnumerable<IIfcWallStandardCase> GetWalls(IfcStore model)
        {
            return IfcWalls = model.Instances.OfType<IIfcWallStandardCase>();
        }

        /// <summary>
        ///Get Wall List from the static IfcWalls Collection
        /// </summary>
        /// <returns></returns>
        public static List<Wall> ExtractWalls()
        {
            List<Wall> wallsList = new List<Wall>();
            foreach (var wall in IfcWalls)
            {

                Wall crntWall=new Wall(new Dimension());
                //get the wall x,y,z
                var recD = wall.Representation.Representations.SelectMany(a => a.Items).OfType<IIfcExtrudedAreaSolid>().Select(a => a.SweptArea).OfType<IIfcRectangleProfileDef>().FirstOrDefault();

                if (recD != null)
                {
                    crntWall.Label = wall.EntityLabel;

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


        private static void CheckExternal(IIfcWallStandardCase wall, Wall crntWall)
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
        private static void GetOpenings(IIfcWallStandardCase wall, Wall crntWall)
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
                    crntWall.Doors.Add(new Door((float)recProfile.XDim, (float)recProfile.YDim, (float)recDepth.FirstOrDefault()
                        , (float)oLocation.X, (float)oLocation.Y, (float)oLocation.Z));
                }
                else
                {

                    crntWall.windows.Add(new Window((float)recProfile.XDim, (float)recProfile.YDim, (float)recDepth.FirstOrDefault()
                        , (float)oLocation.X, (float)oLocation.Y, (float)oLocation.Z));

                }
            }

        }
        private static void GetLocation(IIfcWallStandardCase wall, Wall crntWall)
        {
            //get the wall x,y,z
            // var recD = wall.Representation.Representations.SelectMany(a => a.Items).OfType<IIfcExtrudedAreaSolid>().Select(a => a.SweptArea).OfType<IIfcRectangleProfileDef>().FirstOrDefault();
            //get the wall thickness
            //  var thickness = wall.HasAssociations.OfType<IIfcRelAssociatesMaterial>().OfType<IIfcMaterialLayerSetUsage>().Select(a => a.OffsetFromReferenceLine);//.OfType<IfcPositiveLengthMeasure>();
            var location = ((IIfcAxis2Placement3D)((IIfcLocalPlacement)wall.ObjectPlacement).RelativePlacement).Location;
            //using the Wall Class;
            crntWall.Location = new Location((float)location.X, (float)location.Y, (float)location.Z);

        }
        private static void GetDimension(IIfcWallStandardCase wall, Wall crntWall)
        {
            //get the wall x,y,z
            var recD = wall.Representation.Representations.SelectMany(a => a.Items).OfType<IIfcExtrudedAreaSolid>().Select(a => a.SweptArea).OfType<IIfcRectangleProfileDef>().FirstOrDefault();
            var other = wall.Representation.Representations.SelectMany(a => a.Items).OfType<IIfcBooleanClippingResult>().Select(a => a.FirstOperand).OfType<IIfcExtrudedAreaSolid>().Select(a => a.SweptArea).OfType<IIfcRectangleProfileDef>().FirstOrDefault();
            var otherDepth = wall.Representation.Representations.SelectMany(a => a.Items).OfType<IIfcBooleanClippingResult>().Select(a => a.FirstOperand).OfType<IIfcExtrudedAreaSolid>().Select(a => a.Depth);
            var depth = wall.Representation.Representations.SelectMany(a => a.Items).OfType<IIfcExtrudedAreaSolid>().Select(a => a.Depth);
            //get the wall thickness
            var thickness = wall.HasAssociations.OfType<IIfcRelAssociatesMaterial>().OfType<IIfcMaterialLayerSetUsage>().Select(a => a.OffsetFromReferenceLine);//.OfType<IfcPositiveLengthMeasure>();
            var location = ((IIfcAxis2Placement3D)((IIfcLocalPlacement)wall.ObjectPlacement).RelativePlacement).Location;
            //using the Wall Class;
            crntWall.Dimensions = new Dimension((float)recD.XDim, (float)recD.YDim, (float)depth.FirstOrDefault());
        }
        #endregion

    }
}

