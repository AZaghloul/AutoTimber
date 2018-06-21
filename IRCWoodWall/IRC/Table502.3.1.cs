using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bim.Application.IRCWood.Physical;
using TableCell = Bim.Application.IRCWood.IRC.TableCell502_3_1;
using Table = Bim.Application.IRCWood.IRC.Table502_3_1;
using Bim.Domain.Ifc;
using Bim.Common.Measures;

namespace Bim.Application.IRCWood.IRC
{
    public class Table502_3_1
    {
        public static string FilePath { get; set; }
        public List<TableCell> Cells { get; set; }
        public Table502_3_1()
        {
            Cells = new List<TableCell>();
        }
        public List<TableCell> GetCells(WoodType WT, WoodGrade WG, double DeadLoadPsf, double SpanInInches, double SectionDepth)
        {
            return Cells.Where(e =>
                e.WoodGrade == WG &&
                e.WoodType == WT &&
                e.DeadLoadPsF >= DeadLoadPsf &&
                e.SpanToInch > SpanInInches &&
                e.Section.Depth >= SectionDepth)
                .OrderBy(e => e.Section.Depth)
                .OrderBy(e => e.SpanToInch)
                .ToList();
        }
        public static Table Load(string filePath)
        {
            //get filepath from load function or use the default one
            filePath = filePath ?? FilePath;
            //
            var table = new Table();
            string[] file = File.ReadAllLines(filePath).Where(e => e != ",,,,,,,,").ToArray();

            string[] Keys = file[0].Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
            string[][] cells = new string[file.Length - 1][];
            for (int i = 0; i < cells.Length; i++)
            {
                cells[i] = file[i + 1].Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
            }

            for (int i = 0; i < cells.Length; i++)
            {
                for (int j = 3; j < 7; j++)
                {
                    Double space = Double.Parse(cells[i][0]);
                    TimperSection section = new TimperSection(int.Parse(Keys[j].Split('*')[0]), int.Parse(Keys[j].Split('*')[1]));
                    Length L = Length.FromFeetAndInches(int.Parse(cells[i][j].Split('-')[0]), int.Parse(cells[i][j].Split('-')[1]));
                    //Domain.Ifc.IfDimension Dim = new Domain.Ifc.IfDimension(int.Parse(Keys[j].Split('*')[0]),
                    //    int.Parse(Keys[j].Split('*')[1]),
                    //    int.Parse(cells[i][j].Split('-')[0]) * 12 + int.Parse(cells[i][j].Split('-')[1]));
                    WoodType WT = WoodType.Douglas_fir_larch;
                    WoodGrade WG = new WoodGrade();
                    int DLPsf = int.Parse(cells[i][7]);
                    switch (cells[i][1])
                    {
                        case "Douglas fir-larch":
                            WT = WoodType.Douglas_fir_larch;
                            break;
                        case "Hem-fir":
                            WT = WoodType.Hem_fir;
                            break;
                        case "Southern pine":
                            WT = WoodType.Southern_pine;
                            break;
                        case "Spruce-pine-fir":
                            WT = WoodType.Spruce_pine_fir;
                            break;
                        default:
                            break;
                    }
                    switch (cells[i][2])
                    {
                        case "SS":
                            WG = WoodGrade.SS;
                            break;
                        case "#1":
                            WG = WoodGrade._1;
                            break;
                        case "#2":
                            WG = WoodGrade._2;
                            break;
                        case "#3":
                            WG = WoodGrade._3;
                            break;
                        default:
                            break;
                    }
                    table.Cells.Add(new TableCell()
                    {
                        Spacing = space,
                        Section = section,
                        Span = L,
                        WoodType = WT,
                        WoodGrade = WG,
                        DeadLoadPsF = DLPsf
                    });
                }
            }
            return table;
        }
    }
}
