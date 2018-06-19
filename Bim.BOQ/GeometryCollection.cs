using Bim.Domain.Ifc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bim.BOQ
{
    public class GeometryCollection
    {
        public DataTable BOQTable { get; set; }
        public List<IfElement> ElementCollection { get; set; }
        public List<int> NumberOfElements { get; set; }

        public GeometryCollection()
        {
            ElementCollection = new List<IfElement>();
            NumberOfElements = new List<int>();
            BOQTable = new DataTable("BOQ");
            BOQTable.Columns.Add("Elements Collection",typeof(string));
            BOQTable.Columns.Add("Number", typeof(int));
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
                int index ;
                IfElement E = CheckElement(item);
                if (E != null)
                {
                    index = ElementCollection.IndexOf(E);
                    DataRow DR = BOQTable.Rows[index];
                    DR.SetField<int>("Number", DR.Field<int>("Number")+1);
                    NumberOfElements[index]++;
                }
                else
                {
                    BOQTable.Rows.Add(item.ToString(),1);
                    ElementCollection.Add(item);
                    NumberOfElements.Add(1);
                }
            }
        }
    }
}