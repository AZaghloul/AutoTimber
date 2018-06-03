using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bim.Common.Geometery
{
    public class Split
    {

        public static List<double> Distance(double distance, double maxDistance)
        {
            // double full
            return Distance(0, distance, maxDistance);

        }

        public static List<double> Distance(double start, double distance, double maxDistance, double minValue = .000000001)
        {
            // double full
            List<double> res = new List<double>();
            //<<(1)
            double crntDistance = start;
            res.Add(crntDistance);

            while (distance > crntDistance && crntDistance + maxDistance < distance)
            {
                crntDistance += maxDistance;
                res.Add(crntDistance);
            };

            if (distance > crntDistance && distance - crntDistance > minValue)
            {
                res.Add(crntDistance + distance - crntDistance);
            }
            //>>
            return res;

            //(1)
            //double length = distance - start;
            //int n = (int)(length / maxDistance);
            //double spacing = length / n;
            //for (int i = 0; i < n; i++)
            //{
            //    res.Add(start + i * spacing);
            //}
        }
        public static List< double> Equal(double distance, double maxDistance)
        {
            List<double> res = new List<double>();
            var no = Math.Round(distance / maxDistance);
            var accmul = 0.0;
            var spacings = (distance / no);
            res.Add(accmul);
            for (int i = 0; i < no; i++)
            {
                accmul += spacings;
                res.Add(accmul);
            }
            return res;
        }


    }
}
