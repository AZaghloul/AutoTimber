using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bim.Domain.Ifc;
namespace Bim.Domain.Configuration
{
    public static class Startup
    {

        public static void Configuration(IfModel ifModel)
        {

            IfMaterial.Defaults = new Dictionary<string, IfMaterial>()
            {
                {"Header",new IfMaterial(ifModel,new IfColor(0,255,0)) },
                {"TopPlate",new IfMaterial(ifModel,new IfColor(255,102,255)) },
                {"BottomPlate",new IfMaterial(ifModel,new IfColor(0, 0, 255)) },
                {"Stud",new IfMaterial(ifModel,new IfColor(255, 255, 0)) },
                {"Sill",new IfMaterial(ifModel,new IfColor(0, 255, 255)) },
                {"Plate",new IfMaterial(ifModel,new IfColor(153, 102, 0)) }
            };

        }


        static Startup( )
        {
           
        }

    }
}
