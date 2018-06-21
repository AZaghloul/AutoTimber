using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TableCell = Bim.Application.IRCWood.IRC.TableCell502_5;
using Table = Bim.Application.IRCWood.IRC.Table502_5;
using System.IO;
using Bim.Common.Measures;
using System.Threading;
using Bim.Domain.Ifc;

namespace Bim.Application.IRCWood.IRC
{
    public class Table502_5
    {
        public List<TableCell> Cells { get; set; }
        public Table502_5() => Cells = new List<TableCell>();
        public static Table Load(string filePath)
        {
            Table T = new Table();
            string[] TableDataLines = File.ReadAllLines(filePath).Where(e => e != ",,,,,,,,,,,,,,,,,").ToArray();
            string[] GSnowLoad = TableDataLines[0].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            string[] BuildingWidth = TableDataLines[1].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            string[] HeadersSupporting = TableDataLines[2].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(e => e.Trim('\"')).ToArray();

            string[] vs = TableDataLines[3].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToArray();
            string[][] vss = new string[vs.Length][];

            string[] spans = TableDataLines.Skip(4).ToArray();

            Parallel.For(0, vs.Length,
                i =>
                {
                    vss[i] = vs[i].Split('-', '×');
                });

            //for (int i = 0; i < vs.Length; i++)
            //{
            //    vss[i] = vs[i].Split('-', '×');
            //}

            #region Parallel.For
            //Parallel.For(0, HeadersSupporting.Length,
            //    l =>
            //    {

            //        int NoOfStoriesAbove = 0;
            //        bool clearSpan = true;
            //        switch (HeadersSupporting[l])
            //        {
            //            case "Roof and ceiling":
            //                NoOfStoriesAbove = 0;
            //                clearSpan = true;
            //                break;
            //            case "Roof- ceiling and one center-bearing floor":
            //                NoOfStoriesAbove = 1;
            //                clearSpan = false;
            //                break;
            //            case "Roof- ceiling and one clear span floor":
            //                NoOfStoriesAbove = 1;
            //                clearSpan = true;
            //                break;
            //            case "Roof- ceiling and two center-bearing floors":
            //                NoOfStoriesAbove = 2;
            //                clearSpan = false;
            //                break;
            //            case "Roof- ceiling and two clear span floors":
            //                NoOfStoriesAbove = 2;
            //                clearSpan = true;
            //                break;
            //            case "One floor only":
            //                NoOfStoriesAbove = 1;
            //                break;
            //            case "Two floors":
            //                NoOfStoriesAbove = 2;
            //                break;
            //            default:
            //                break;
            //        }

            //        Parallel.For(0, spans.Length / HeadersSupporting.Length,
            //            i =>
            //            {
            //                string[] spanLineSplitted = spans[(l * spans.Length / HeadersSupporting.Length) + i].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            //                int noHeaders = Convert.ToInt32(vss[i][0]);
            //                int HeaderWidth = Convert.ToInt32(vss[i][1]);
            //                int HeaderDepth = Convert.ToInt32(vss[i][2]);

            //                Parallel.For(0, GSnowLoad.Length,
            //                    j =>
            //                    {
            //                        int GroundSL = Convert.ToInt32(GSnowLoad[j]);
            //                        Parallel.For(0, BuildingWidth.Length,
            //                            k =>
            //                            {
            //                                Length BuildingW = Length.FromFeet(Convert.ToInt32(BuildingWidth[k]));

            //                                Length Span;
            //                                string[] spanSplitted = spanLineSplitted[(j * BuildingWidth.Length + k) * 2].Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
            //                                Span = Length.FromFeetAndInches(Convert.ToInt32(spanSplitted[0]), Convert.ToInt32(spanSplitted[1]));
            //                                int noJackStuds = Convert.ToInt32(spanLineSplitted[(j * BuildingWidth.Length + k) * 2 + 1]);
            //                                TableCell Cell = new TableCell(Span, new TimperSection(HeaderWidth, HeaderDepth), noHeaders, noJackStuds,
            //                                    BuildingW, GroundSL, NoOfStoriesAbove, clearSpan);
            //                                if (Cell != null)
            //                                {
            //                                    T.Cells.Add(Cell);
            //                                }
            //                                else
            //                                {
            //                                    throw new NullReferenceException();
            //                                }
            //                            }
            //                            );

            //                    }
            //                    );

            //            }
            //            );


            //    }
            //    );

            #endregion

            for (int l = 0; l < HeadersSupporting.Length; l++)
            {
                int NoOfStoriesAbove = 0;
                bool clearSpan = true;
                switch (HeadersSupporting[l])
                {
                    case "Roof and ceiling":
                        NoOfStoriesAbove = 0;
                        clearSpan = true;
                        break;
                    case "Roof- ceiling and one center-bearing floor":
                        NoOfStoriesAbove = 1;
                        clearSpan = false;
                        break;
                    case "Roof- ceiling and one clear span floor":
                        NoOfStoriesAbove = 1;
                        clearSpan = true;
                        break;
                    case "Roof- ceiling and two center-bearing floors":
                        NoOfStoriesAbove = 2;
                        clearSpan = false;
                        break;
                    case "Roof- ceiling and two clear span floors":
                        NoOfStoriesAbove = 2;
                        clearSpan = true;
                        break;
                    case "One floor only":
                        NoOfStoriesAbove = 1;
                        break;
                    case "Two floors":
                        NoOfStoriesAbove = 2;
                        break;
                    default:
                        break;
                }
                for (int i = 0; i < spans.Length / HeadersSupporting.Length; i++)
                {
                    string[] spanLineSplitted = spans[(l * spans.Length / HeadersSupporting.Length) + i].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                    int noHeaders = Convert.ToInt32(vss[i][0]);
                    int HeaderWidth = Convert.ToInt32(vss[i][1]);
                    int HeaderDepth = Convert.ToInt32(vss[i][2]);

                    for (int j = 0; j < GSnowLoad.Length; j++)
                    {
                        int GroundSL = Convert.ToInt32(GSnowLoad[j]);
                        for (int k = 0; k < BuildingWidth.Length; k++)
                        {
                            Length BuildingW = Length.FromFeet(Convert.ToInt32(BuildingWidth[k]));

                            Length Span;
                            string[] spanSplitted = spanLineSplitted[(j * BuildingWidth.Length + k) * 2].Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
                            Span = Length.FromFeetAndInches(Convert.ToInt32(spanSplitted[0]), Convert.ToInt32(spanSplitted[1]));
                            int noJackStuds = Convert.ToInt32(spanLineSplitted[(j * BuildingWidth.Length + k) * 2 + 1]);
                            T.Cells.Add(new TableCell(Span, new RecSection(HeaderWidth, HeaderDepth), noHeaders, noJackStuds,
                                BuildingW, GroundSL, NoOfStoriesAbove, clearSpan));
                        }
                    }
                }
            }

            return T;
        }
    }
}