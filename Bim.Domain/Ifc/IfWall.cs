using System.Collections.Generic;
using System.Linq;
using Xbim.Ifc;
using Xbim.Ifc4.Interfaces;

namespace Bim.Domain.Ifc
{
    /// <summary>
    /// Wall object to Carry IFCStandardWall Data
    /// </summary>

    public class IfWall : IfElement
    {
        #region wall Object Properties
       
        public IfStory Story { get; set; }
        public List<IDoor> Doors { get; set; }
        public List<IWindow> Windows { get; set; }
        public List<IfOpening> Openings { get; set; }
        public bool IsExternal { get; set; }
        public IIfcWallStandardCase IfcWall { get; set; }
        public IIfcAxis2Placement3D WallAxis { get; set; }
        public IIfcLocalPlacement LocalPlacement { get; set; }
        #endregion

        #region Constructors
        public IfWall()
        {

        }
        public IfWall(IfModel ifModel,IIfcWallStandardCase ifcWall)
        {
            IfcWall = ifcWall;
            IfModel = ifModel;
            IfModel.Instances.Add(this);
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
                    IfWall crntWall = new IfWall(ifStory.IfModel,wall);
                    crntWall.Story = ifStory;
                    crntWall.IfModel = ifStory.IfModel;
                    wallsList.Add(crntWall);
                }
            }

            return wallsList;
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
            var res = (bool)IfcWall.IsDefinedBy
                    .Where(r => r.RelatingPropertyDefinition is IIfcPropertySet)
                    .SelectMany(r => ((IIfcPropertySet)r.RelatingPropertyDefinition).HasProperties)
                    .OfType<IIfcPropertySingleValue>().Where(a => a.Name == "IsExternal").Select(a => a.NominalValue).FirstOrDefault().Value;
            if (res == true)
            {
                IsExternal = true;
            }
            else
            {
                IsExternal = false;
            }

        }
        private void GetLocation()
        {
            WallAxis = ((IIfcAxis2Placement3D)((IIfcLocalPlacement)IfcWall.ObjectPlacement).RelativePlacement);
            var location = WallAxis.Location;
            LocalPlacement = (IIfcLocalPlacement)IfcWall.ObjectPlacement;
            IfLocation = new IfLocation((float)location.X, (float)location.Y, (float)location.Z);
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
                         .OfType<IIfcRectangleProfileDef>().FirstOrDefault();

            var depth = IfcWall.Representation.Representations
                         .SelectMany(a => a.Items)
                         .OfType<IIfcExtrudedAreaSolid>().Select(a => a.Depth) ??
                        IfcWall.Representation.Representations
                            .SelectMany(a => a.Items)
                            .OfType<IIfcBooleanClippingResult>().Select(a => a.FirstOperand)
                            .OfType<IIfcExtrudedAreaSolid>().Select(a => a.Depth);


            //var recD = wall.Representation.Representations.SelectMany(a => a.Items).OfType<IIfcExtrudedAreaSolid>().Select(a => a.SweptArea).OfType<IIfcRectangleProfileDef>().FirstOrDefault();
            //var recD1 = wall.Representation.Representations.SelectMany(a => a.Items).OfType<IIfcBooleanClippingResult>().Select(a => a.FirstOperand).OfType<IIfcExtrudedAreaSolid>().Select(a => a.SweptArea).OfType<IIfcRectangleProfileDef>().FirstOrDefault();
            //var otherDepth = wall.Representation.Representations.SelectMany(a => a.Items).OfType<IIfcBooleanClippingResult>().Select(a => a.FirstOperand).OfType<IIfcExtrudedAreaSolid>().Select(a => a.Depth);
            //var depth = wall.Representation.Representations.SelectMany(a => a.Items).OfType<IIfcExtrudedAreaSolid>().Select(a => a.Depth);
            //get the wall thickness
            var thickness = IfcWall.HasAssociations.OfType<IIfcRelAssociatesMaterial>().OfType<IIfcMaterialLayerSetUsage>().Select(a => a.OffsetFromReferenceLine);//.OfType<IfcPositiveLengthMeasure>();
            var location = ((IIfcAxis2Placement3D)((IIfcLocalPlacement)IfcWall.ObjectPlacement).RelativePlacement).Location;
            //using the Wall Class;
            if (recD != null && depth != null)
            {
                IfDimension= new IfDimension((float)recD.XDim, (float)recD.YDim, (float)depth.FirstOrDefault());
            }
        }
        #endregion



    }

}

