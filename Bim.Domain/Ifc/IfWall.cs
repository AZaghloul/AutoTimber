using System.Collections.Generic;
using System.Linq;
using Xbim.Ifc;
using Xbim.Ifc4.Interfaces;
using Xbim.Ifc4.SharedBldgElements;

namespace Bim.Domain.Ifc
{
    /// <summary>
    /// Wall object to Carry IFCStandardWall Data
    /// </summary>

    public class IfWall : IfElement
    {
        public static Option DetectExternalWalls { get; set; } = Option.Auto;
        #region wall Object Properties

        public IfStory Story { get; set; }
        public List<IDoor> Doors { get; set; }
        public List<IWindow> Windows { get; set; }
        public List<IfOpening> Openings { get; set; }
        public bool IsExternal { get; set; }
        public IIfcAxis2Placement3D WallAxis { get; set; }
        public IIfcLocalPlacement LocalPlacement { get; set; }
        public Direction Direction { get; set; }
        #endregion

        #region Constructors
        public IfWall()
        {

        }
        public IfWall(IfModel ifModel, IIfcWallStandardCase ifcWall)
        {
            base.IfcElement = (IfcWallStandardCase)ifcWall;
            IfModel = ifModel;
            IfModel.WallCollection.Add(this);
            Initialize();
        }


        #endregion

        #region  static Methods
        public static List<IfWall> GetWalls(IfStory ifStory)
        {
            List<IfWall> wallsList = new List<IfWall>();
            var walls = ifStory.IfcStory.ContainsElements
                 .FirstOrDefault()
                 .RelatedElements.OfType<IIfcWallStandardCase>();

            foreach (var wall in walls)
            {
                var dir = ((IIfcAxis2Placement3D)((IIfcLocalPlacement)wall.ObjectPlacement).RelativePlacement).RefDirection;
                var recD = wall.Representation.Representations
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
                    IfWall crntWall = new IfWall(ifStory.IfModel, wall)
                    {
                        Story = ifStory,
                        IfModel = ifStory.IfModel
                    };

                    if (dir != null && dir.X < 0)
                    {
                        crntWall.Direction = Direction.Negative;
                    }
                    else
                    {
                        crntWall.Direction = Direction.Positive;
                    }


                    wallsList.Add(crntWall);
                }
            }

            return wallsList;
        }

        public static void SetExternalWalls(List<IfWall> walls)
        {

        }
        #endregion

        #region Helper Private Functions
        private void Initialize()
        {
            Guid = IfcElement.GlobalId;
            Name = IfcElement.Name;
            Label = IfcElement.EntityLabel;
            GetLocation();
            CheckExternal();
            GetDimension();
            if (Openings != null) return;

            Openings = IfOpening.GetOpenings(this);
        }

        private void CheckExternal()
        {
            IsExternal = (bool)IfcElement.IsDefinedBy
                   .Where(r => r.RelatingPropertyDefinition is IIfcPropertySet)
                   .SelectMany(r => ((IIfcPropertySet)r.RelatingPropertyDefinition).HasProperties)
                   .OfType<IIfcPropertySingleValue>().
                   Where(a => a.Name == "IsExternal").
                   Select(a => a.NominalValue).FirstOrDefault().Value;
        }
        private void GetLocation()
        {
            WallAxis = ((IIfcAxis2Placement3D)((IIfcLocalPlacement)IfcElement.ObjectPlacement).RelativePlacement);
            var location = WallAxis.Location;
            LocalPlacement = (IIfcLocalPlacement)IfcElement.ObjectPlacement;
            IfLocation = new IfLocation((float)location.X, (float)location.Y, (float)location.Z);
        }
        private void GetDimension()
        {
            //get the wall x,y,z
            var recD = IfcElement.Representation.Representations
                         .SelectMany(a => a.Items)
                         .OfType<IIfcExtrudedAreaSolid>().Select(a => a.SweptArea)
                         .OfType<IIfcRectangleProfileDef>().FirstOrDefault() ??
                       IfcElement.Representation.Representations
                         .SelectMany(a => a.Items)
                         .OfType<IIfcBooleanClippingResult>().Select(a => a.FirstOperand)
                         .OfType<IIfcExtrudedAreaSolid>().Select(a => a.SweptArea)
                         .OfType<IIfcRectangleProfileDef>().FirstOrDefault();

            var depth =
                 IfcElement.Representation.Representations
                        .SelectMany(a => a.Items)
                        .OfType<IIfcExtrudedAreaSolid>().Select(a => a.Depth).FirstOrDefault();

            if (depth == 0)
            {
                try
                {
                    depth = ((IIfcExtrudedAreaSolid)IfcElement.Representation.Representations
                 .SelectMany(a => a.Items)
                 .OfType<IIfcBooleanClippingResult>().
                 Select(a => a.FirstOperand).FirstOrDefault()).Depth;
                }
                catch (System.Exception)
                {


                }


            }



            //var recD = wall.Representation.Representations.SelectMany(a => a.Items).OfType<IIfcExtrudedAreaSolid>().Select(a => a.SweptArea).OfType<IIfcRectangleProfileDef>().FirstOrDefault();
            //var recD1 = wall.Representation.Representations.SelectMany(a => a.Items).OfType<IIfcBooleanClippingResult>().Select(a => a.FirstOperand).OfType<IIfcExtrudedAreaSolid>().Select(a => a.SweptArea).OfType<IIfcRectangleProfileDef>().FirstOrDefault();
            //var otherDepth = wall.Representation.Representations.SelectMany(a => a.Items).OfType<IIfcBooleanClippingResult>().Select(a => a.FirstOperand).OfType<IIfcExtrudedAreaSolid>().Select(a => a.Depth);
            //var depth = wall.Representation.Representations.SelectMany(a => a.Items).OfType<IIfcExtrudedAreaSolid>().Select(a => a.Depth);
            //get the wall thickness
            var thickness = IfcElement.HasAssociations.OfType<IIfcRelAssociatesMaterial>().OfType<IIfcMaterialLayerSetUsage>().Select(a => a.OffsetFromReferenceLine);//.OfType<IfcPositiveLengthMeasure>();
            var location = ((IIfcAxis2Placement3D)((IIfcLocalPlacement)IfcElement.ObjectPlacement).RelativePlacement).Location;
            //using the Wall Class;
            if (recD != null && depth != null)
            {
                IfDimension = new IfDimension(recD.XDim, recD.YDim, depth);
            }
        }
        private void GetDirection()
        {

        }
        #endregion



    }

}

