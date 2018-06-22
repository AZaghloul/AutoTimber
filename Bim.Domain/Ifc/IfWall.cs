using System.Collections.Generic;
using System.Linq;
using Xbim.Ifc;
using Xbim.Ifc4.Interfaces;
using Bim.Common.Measures;
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
        public IIfcWall IfcWall { get; set; }
        public IIfcAxis2Placement3D WallAxis { get; set; }
        public IIfcLocalPlacement LocalPlacement { get; set; }
        public Direction Direction { get; set; }
        #endregion

        #region Constructors
        public IfWall() : base(null)
        {

        }
        public IfWall(IfStory ifStory, IIfcWall ifcWall) : base(ifStory.IfModel)
        {
            IfcWall = ifcWall;
            Story = ifStory;
            Initialize();
        }


        #endregion

        #region  static Methods
        public static List<IfWall> GetWalls(IfStory ifStory)
        {
            List<IfWall> wallsList = new List<IfWall>();
            var walls = ifStory.IfcStory.ContainsElements
                 .FirstOrDefault()
                 .RelatedElements.OfType<IIfcWall>();


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
                                .OfType<IIfcRectangleProfileDef>().FirstOrDefault() ??
                            wall.Representation.Representations
                                .SelectMany(a => a.Items)
                                .OfType<IIfcBooleanClippingResult>().Select(a => a.FirstOperand)
                                .OfType<IIfcBooleanClippingResult>().Select(a => a.FirstOperand)
                                .OfType<IIfcExtrudedAreaSolid>().Select(a => a.SweptArea)
                                .OfType<IIfcRectangleProfileDef>().FirstOrDefault() ??
                            wall.Representation.Representations
                                .SelectMany(a => a.Items)
                                .OfType<IIfcBooleanClippingResult>().Select(a => a.FirstOperand)
                                .OfType<IIfcBooleanClippingResult>().Select(a => a.FirstOperand)
                                .OfType<IIfcBooleanClippingResult>().Select(a => a.FirstOperand)
                                .OfType<IIfcExtrudedAreaSolid>().Select(a => a.SweptArea)
                                .OfType<IIfcRectangleProfileDef>().FirstOrDefault();


                //get the wall x,y,z
                if (recD != null)
                {
                    IfWall crntWall = new IfWall(ifStory, wall);
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
            Guid = IfcWall.GlobalId;
            Name = IfcWall.Name;
            Label = IfcWall.EntityLabel;
            GetLocation();
            CheckExternal();
            GetDimension();
            if (Openings != null) return;

            Openings = IfOpening.GetOpenings(this);
        }

        private void CheckExternal()
        {
            IsExternal = (bool)IfcWall.IsDefinedBy
                   .Where(r => r.RelatingPropertyDefinition is IIfcPropertySet)
                   .SelectMany(r => ((IIfcPropertySet)r.RelatingPropertyDefinition).HasProperties)
                   .OfType<IIfcPropertySingleValue>().
                   Where(a => a.Name == "IsExternal").
                   Select(a => a.NominalValue).FirstOrDefault().Value;
        }
        private void GetLocation()
        {
            var polylineStartPoint = IfcWall.Representation.Representations.SelectMany(a => a.Items).OfType<IIfcPolyline>().FirstOrDefault().Points[0];
            LocalPlacement = (IIfcLocalPlacement)IfcWall.ObjectPlacement;
            WallAxis = ((IIfcAxis2Placement3D)(LocalPlacement.RelativePlacement));
            var location = WallAxis.Location;
            IfLocation = new IfLocation(Length.FromFeet(polylineStartPoint.X).Inches, Length.FromFeet(polylineStartPoint.Y).Inches, Length.FromFeet(location.Z).Inches);
        }
        private void GetDimension()
        {
            //get the wall x,y,z
            var recD = IfcWall.Representation.Representations
                         .SelectMany(a => a.Items)
                         .OfType<IIfcExtrudedAreaSolid>().Select(a => a.SweptArea)
                         .OfType<IIfcRectangleProfileDef>().FirstOrDefault() ??
                       IfcWall.Representation.Representations
                         .SelectMany(a => a.Items)
                         .OfType<IIfcBooleanClippingResult>().Select(a => a.FirstOperand)
                         .OfType<IIfcExtrudedAreaSolid>().Select(a => a.SweptArea)
                         .OfType<IIfcRectangleProfileDef>().FirstOrDefault() ??
                       IfcWall.Representation.Representations
                         .SelectMany(a => a.Items)
                         .OfType<IIfcBooleanClippingResult>().Select(a => a.FirstOperand)
                         .OfType<IIfcBooleanClippingResult>().Select(a => a.FirstOperand)
                         .OfType<IIfcExtrudedAreaSolid>().Select(a => a.SweptArea)
                         .OfType<IIfcRectangleProfileDef>().FirstOrDefault() ??
                      IfcWall.Representation.Representations
                          .SelectMany(a => a.Items)
                          .OfType<IIfcBooleanClippingResult>().Select(a => a.FirstOperand)
                          .OfType<IIfcBooleanClippingResult>().Select(a => a.FirstOperand)
                          .OfType<IIfcBooleanClippingResult>().Select(a => a.FirstOperand)
                          .OfType<IIfcExtrudedAreaSolid>().Select(a => a.SweptArea)
                          .OfType<IIfcRectangleProfileDef>().FirstOrDefault();



            var depth =
                 IfcWall.Representation.Representations
                        .SelectMany(a => a.Items)
                        .OfType<IIfcExtrudedAreaSolid>().Select(a => a.Depth).FirstOrDefault();
            if (depth == 0)
                depth =
                 IfcWall.Representation.Representations
                        .SelectMany(a => a.Items)
                        .OfType<IIfcBooleanClippingResult>().Select(a => a.FirstOperand)
                        .OfType<IIfcExtrudedAreaSolid>().Select(a => a.Depth).FirstOrDefault();
            if (depth == 0)
                depth =
                 IfcWall.Representation.Representations
                        .SelectMany(a => a.Items)
                        .OfType<IIfcBooleanClippingResult>().Select(a => a.FirstOperand)
                        .OfType<IIfcBooleanClippingResult>().Select(a => a.FirstOperand)
                        .OfType<IIfcExtrudedAreaSolid>().Select(a => a.Depth).FirstOrDefault();
            if (depth == 0)
                depth =
                 IfcWall.Representation.Representations
                        .SelectMany(a => a.Items)
                        .OfType<IIfcBooleanClippingResult>().Select(a => a.FirstOperand)
                        .OfType<IIfcBooleanClippingResult>().Select(a => a.FirstOperand)
                        .OfType<IIfcBooleanClippingResult>().Select(a => a.FirstOperand)
                        .OfType<IIfcExtrudedAreaSolid>().Select(a => a.Depth).FirstOrDefault();


            if (depth == 0)
            {
                try
                {
                    depth = ((IIfcExtrudedAreaSolid)IfcWall.Representation.Representations
                 .SelectMany(a => a.Items)
                 .OfType<IIfcBooleanClippingResult>().
                 Select(a => a.FirstOperand).FirstOrDefault()).Depth;
                }
                catch (System.Exception)
                {


                }


            }
            var thickness = IfcWall.HasAssociations.OfType<IIfcRelAssociatesMaterial>().OfType<IIfcMaterialLayerSetUsage>().Select(a => a.OffsetFromReferenceLine);//.OfType<IfcPositiveLengthMeasure>();
            //var location = ((IIfcAxis2Placement3D)((IIfcLocalPlacement)IfcWall.ObjectPlacement).RelativePlacement).Location;
            //using the Wall Class;
            if (recD != null && depth != null)
            {
                IfDimension = new IfDimension(Length.FromFeet(recD.XDim).Inches, Length.FromFeet(recD.YDim).Inches, Story.StoryHeight);
            }
        }
        #endregion
    }
}