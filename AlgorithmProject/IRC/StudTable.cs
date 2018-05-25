using Bim.Domain.Ifc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmProject
{
    public class StudTable
    {
        public string[] Headers { get; set; }
        public string[] Keys { get; set; }
        public int Floors { get; set; }
        public List<StudCell> Cells { get; set; }
        public StudTable()
        {
            Cells = new List<StudCell>();
            Floors = 3;
        }

        public bool LoadTable(string filePath)
        {
            string[] data = File.ReadAllLines(filePath);
            Headers = data[0].Split(',');
            Keys = data[1].Split(',');

            string[] values = null;
            for (int k = 0; k < Floors; k++)
            {
                switch (k)
                {
                    case 0:
                        values = data.Skip(2).ToArray();
                        break;
                    case 1:
                        values = data.Skip(10).ToArray();
                        break;
                    case 2:
                        values = data.Skip(18).ToArray();
                        break;

                    default:
                        break;
                }

                for (int i = 0; i < Keys.Length; i++)
                {
                    for (int j = 0; j < Headers.Length; j++)
                    {
                        string[] dimRaw = values[i].Split(',');
                        var dim = dimRaw[j].Split(' ');

                        var x = Convert.ToDouble(dim[0]);
                        var y = Convert.ToDouble(dim[1]);
                        var cell = new StudCell(
                            Headers[j],
                            Convert.ToInt16(Keys[i]),
                            new IfDimension((float)x, (float)y, 0)
                            );

                        cell.Floor = k + 1;
                        Cells.Add(cell);
                    }
                }

            }
            return true;

        }

        public List<StudCell> GetSpace(int floor, int height, IfDimension dimension)
        {

           return Cells.Where(e =>
            {
                return e.Floor == floor && e.Height == height&&
               e.Value.XDim==dimension.XDim&&e.Value.YDim==dimension.YDim ;





                }).ToList();

        }



    }
}
