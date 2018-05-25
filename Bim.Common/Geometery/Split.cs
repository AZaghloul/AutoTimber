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
            List<double> res = new List<double>();
            double crntDistance = 0;
            while (distance > crntDistance && crntDistance + maxDistance < distance)
            {
                crntDistance += maxDistance;
                res.Add(crntDistance);
            };

            if (distance > crntDistance)
            {
                res.Add(crntDistance + distance - crntDistance);
            }
            return res;

        }


    }
}
