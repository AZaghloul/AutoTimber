using Bim.Common.Measures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xbim.Ifc4.Interfaces;

namespace Bim.Domain.Ifc
{
    public class IfOpening2 : IfElement
    {
        public IfWall2 IfWall { get; set; }
        public IfDirection IfDirection { get; set; }
        public OpeningType OpeningType { get; set; }
        public IIfcOpeningElement IfcOpening
        {
            get
            {
                return (IIfcOpeningElement)IfcElement;
            }
            set
            {
                IfcElement = value;
            }
        }
        public IfOpening2() : base(null)
        {

        }
        public IfOpening2(IfWall2 ifWall, IIfcRelVoidsElement ifcOpening) : base(ifWall.IfModel)
        {
            IfWall = ifWall;
            IfcOpening = (IIfcOpeningElement)ifcOpening.RelatedOpeningElement;

            var opnng = (IIfcAxis2Placement3D)((IIfcLocalPlacement)ifcOpening
                .RelatedOpeningElement.ObjectPlacement).RelativePlacement;

            var oLocation = opnng.Location;//get opening location point
            IfLocation = new IfLocation(Length.FromFeet(oLocation.X), Length.FromFeet(oLocation.Y), Length.FromFeet(oLocation.Z));

            var voids = ((IIfcOpeningElement)ifcOpening.RelatedOpeningElement)
                .HasFillings.FirstOrDefault();

            var VoidLocalPlacementRefDirection = ((IIfcAxis2Placement3D)((IIfcLocalPlacement)voids.RelatedBuildingElement.ObjectPlacement).RelativePlacement).RefDirection;

            if (VoidLocalPlacementRefDirection != null)
                IfDirection = new IfDirection(VoidLocalPlacementRefDirection.X, VoidLocalPlacementRefDirection.Y, VoidLocalPlacementRefDirection.Z);
            else IfDirection = new IfDirection(1, 0, 0);

            var recProfile = ifcOpening.RelatedOpeningElement.Representation.Representations.SelectMany(a => a.Items)
                .OfType<IIfcExtrudedAreaSolid>().Select(a => a.SweptArea)
                .OfType<IIfcRectangleProfileDef>().FirstOrDefault(); //get rec profile

            var recDepth = ifcOpening.RelatedOpeningElement.Representation.
                Representations.SelectMany(a => a.Items).
                OfType<IIfcExtrudedAreaSolid>().Select(a => a.Depth).FirstOrDefault();
            //get filling elment doors or windows
            IfDimension = new IfDimension(Length.FromFeet(recProfile.YDim).Inches, Length.FromFeet(recDepth).Inches, Length.FromFeet(recProfile.XDim).Inches);

            string filling = " ";
            if (voids != null)
            {
                filling = voids.RelatedBuildingElement.GetType().Name;
            }
            else
            {
                filling = "not Defined";
            }
            switch (filling)
            {
                case "IfcDoor":
                    OpeningType = OpeningType.Door;
                    IfDimension = new IfDimension(Length.FromFeet(recProfile.XDim).Inches, Length.FromFeet(recDepth).Inches, Length.FromFeet(recProfile.YDim).Inches);

                    break;
                case "IfcWindow":
                    OpeningType = OpeningType.Window;
                    IfDimension = new IfDimension(Length.FromFeet(recProfile.YDim).Inches, Length.FromFeet(recDepth).Inches, Length.FromFeet(recProfile.XDim).Inches);
                    break;
                default:
                    OpeningType = OpeningType.Window;
                    break;
            }

        }
        public IfOpening2(IfOpening2 opening) : base(opening.IfModel)
        {
            IfWall = opening.IfWall;
            OpeningType = opening.OpeningType;
            IfcOpening = opening.IfcOpening;
            IfLocation = new IfLocation(opening.IfLocation);
            IfDimension = new IfDimension(opening.IfDimension);
        }
        public static List<IfOpening2> GetOpenings(IfWall2 ifWall)
        {
            List<IfOpening2> openings = new List<IfOpening2>();
            IfOpening2 ifopening;

            foreach (var opening in ifWall.IfcWall.HasOpenings)
            {
                ifopening = new IfOpening2(ifWall, opening);
                openings.Add(ifopening);
            }
            return openings;
        }
    }
}
