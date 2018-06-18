using Bim.Domain.Ifc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bim.BOQ
{
    public class GeometryCollection
    {
        public List<IfElement> ElementCollection { get; set; }
        public List<int> NumberOfElements { get; set; }

        public GeometryCollection()
        {
            ElementCollection = new List<IfElement>();
            NumberOfElements = new List<int>();
        }

        public IfElement CheckElement(IfElement ifElement)
        {
            foreach (var E in ElementCollection)
            {
                if (E.IfDimension == ifElement.IfDimension)
                    return E;
            }
            return null;
        }

        public void AddToCollection(IEnumerable<IfElement> ifElements)
        {
            foreach (var item in ifElements)
            {
                IfElement E = CheckElement(item);
                if (E != null)
                {
                    int index = ElementCollection.IndexOf(E);
                    NumberOfElements[index]++;
                }
                else
                {
                    ElementCollection.Add(item);
                    NumberOfElements.Add(1);
                }
            }
        }
    }
}
