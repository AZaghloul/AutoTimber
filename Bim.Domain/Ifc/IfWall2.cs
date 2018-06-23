using Bim.Common.Measures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xbim.Ifc4.Interfaces;
using Xbim.Ifc4.SharedBldgElements;

namespace Bim.Domain.Ifc
{
    public class IfWall2 : IfElement
    {
        public IfStory Story { get; set; }
        public List<IfOpening2> Openings { get; set; }
        public IfDirection IfDirection { get; set; }
        public bool IsExternal { get; set; }
        public IIfcWall IfcWall
        {
            get
            {
                return (IIfcWall)IfcElement;
            }
            set
            {
                IfcElement = value;
            }
        }

        #region Ctor
        public IfWall2() : base(null)
        {

        }
        public IfWall2(IfStory ifStory, IIfcWall ifcWall) : base(ifStory.IfModel)
        {
            IfcWall = ifcWall;
            Story = ifStory;
            Initialize();
        }

        #endregion

        #region Intializers
        private void Initialize()
        {
            Guid = IfcWall.GlobalId;
            Name = IfcWall.Name;
            Label = IfcWall.EntityLabel;
            GetLocation();
            CheckExternal();
            GetDimension();
            if (Openings != null) return;

            Openings = IfOpening2.GetOpenings(this);
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
            var LocalPlacement = (IIfcLocalPlacement)IfcWall.ObjectPlacement;
            var RelativePlacement = ((IIfcAxis2Placement3D)LocalPlacement.RelativePlacement);
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

            IfLocation = new IfLocation(Length.FromFeet(RelativePlacement.Location.X), Length.FromFeet(RelativePlacement.Location.Y), Length.FromFeet(RelativePlacement.Location.Z));
            IfDirection = new IfDirection(recD.Position.RefDirection.X, recD.Position.RefDirection.Y, 0);
        }

        #endregion

        public static List<IfWall2> GetWalls(IfStory ifStory)
        {
            List<IfWall2> wallsList = new List<IfWall2>();
            var walls = ifStory.IfcStory.ContainsElements
                 .FirstOrDefault()
                 .RelatedElements.OfType<IIfcWall>();
            foreach (var wall in walls)
            {
                wallsList.Add(new IfWall2(ifStory, wall));
            }
            return wallsList;
        }

    }
}
