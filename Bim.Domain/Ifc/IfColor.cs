using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bim.Domain.Ifc
{
    public class IfColor : IfObject
    {
        public double Red { get; set; }
        public double Green { get; set; }
        public double Blue { get; set; }

        public static IfColor GetColor( IfColorEnum ifColorEnum)
        {
            IfColor res=null;
            switch (ifColorEnum)
            {
                case IfColorEnum.Red:
                    res = new IfColor(1, 0, 0);
                    break;
                case IfColorEnum.Green:
                    res = new IfColor(0, 1, 0);
                    break;
                case IfColorEnum.Blue:
                    res = new IfColor(0, 0, 1);
                    break;
                case IfColorEnum.DeepSkyBlue:
                    res = new IfColor(1, 0, 0);
                    break;
                case IfColorEnum.Pink:
                    res = new IfColor(255, 20, 147);
                    break;
                case IfColorEnum.Yellow:
                    res = new IfColor(1, 0, 0);
                    break;
                case IfColorEnum.Orange:
                    res = new IfColor(255, 69, 0);
                    break;
                case IfColorEnum.Tomato:
                    res = new IfColor(255, 99, 71);
                    break;
                case IfColorEnum.MediumPurple:
                    res = new IfColor(147, 112, 219);
                    break;
                case IfColorEnum.Violet:
                    res = new IfColor(238, 130, 238);
                    break;
                case IfColorEnum.Turquoise:
                    res = new IfColor(173, 234, 234);
                    break;
                case IfColorEnum.DarkGolden:
                    res = new IfColor(184, 134, 11);
                    break;
                case IfColorEnum.LightSlateGrey:
                    res = new IfColor(119, 136, 153);
                    break;
                case IfColorEnum.DodgerBlue:
                    res = new IfColor(30,144, 255);
                    break;
            }
            return res;
        }
        public IfColor(double r, double g, double b)
        {
            if (r > 1 || g > 1 || b > 1)
            {
                Red = r / 255;
                Green = g / 255;
                Blue = b / 255;
            }
            else
            {
                Red = r;
                Green = g;
                Blue = b;
            }
        }
    }

    public enum IfColorEnum
    {
        Red,
        Green,
        Blue,
        DeepSkyBlue,
        Pink,
        Yellow,
        Orange,
        Tomato,
        MediumPurple,
        Violet,
        Turquoise,
        DarkGolden,
        LightSlateGrey,
        DodgerBlue

    }
}
