using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bim.Domain;
using Xbim.Ifc4.Interfaces;
namespace Bim.Domain.Ifc
{
    public class IfOpening : IfElement
    {

        public IfWall IfWall { get; set; }
        public OpeningType OpeningType { get; set; }
        public IIfcRelVoidsElement IfcOpening { get; set; }
        public IElement WallOrSlap { get; set; }

        public IfOpening()
        {

        }
        public IfOpening(IfWall ifWall, IIfcRelVoidsElement ifcOpening)
        {
            IfWall = ifWall;
            IfModel = IfWall.IfModel;
            IfcOpening = ifcOpening;
            IfModel.Instances.Add(this);
        }

        public static List<IfOpening> GetOpenings(IfWall ifWall)
        {
            var openings = new List<IfOpening>();
            IfOpening ifopening;
            foreach (var opening in ifWall.IfcWall.HasOpenings)
            {
                ifopening = new IfOpening(ifWall, opening);

                var opnng = (IIfcAxis2Placement3D)((IIfcLocalPlacement)opening
                    .RelatedOpeningElement.ObjectPlacement).RelativePlacement;
                var oLocation = opnng.Location;
                var recProfile = opening.RelatedOpeningElement.Representation.Representations.SelectMany(a => a.Items)
                    .OfType<IIfcExtrudedAreaSolid>().Select(a => a.SweptArea)
                    .OfType<IIfcRectangleProfileDef>().FirstOrDefault();

                var recDepth = opening.RelatedOpeningElement.Representation.
                    Representations.SelectMany(a => a.Items).
                    OfType<IIfcExtrudedAreaSolid>().Select(a => a.Depth).FirstOrDefault();

                var voids = ((IIfcOpeningElement)opening.RelatedOpeningElement)
                    .HasFillings.FirstOrDefault();
                string filling = " ";
                if (voids != null)
                {
                    filling = voids.RelatedBuildingElement.GetType().Name;
                }
                else
                {
                    filling = "not Defined";
                }


                ifopening.IfLocation = new IfLocation(oLocation.X, oLocation.Y, oLocation.Z);
                ifopening.IfDimension = new IfDimension(recProfile.YDim, recDepth, recProfile.XDim);

                switch (filling)
                {
                    case "IfcDoor":
                        ifopening.OpeningType = OpeningType.Door;
                        break;
                    case "IfcWindow":
                        ifopening.OpeningType = OpeningType.Window;
                        break;
                    default:
                        ifopening.OpeningType = OpeningType.Window;
                        break;

                }



                openings.Add(ifopening);
            }

            return openings;
        }
    }
}
