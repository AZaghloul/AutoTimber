using Bim.Common.Measures;
using Bim.Domain.General;
using Bim.Domain.Ifc;

namespace Bim.Domain.Configuration
{
    public static class Startup
    {

        public static void Configuration(IfModel ifModel)
        {
            //configure the Length class
            Length.RoundValue = 4;

            IfMaterial.Setup = new Setup()
            {
                {"Header",new IfMaterial(ifModel,new IfColor(0,255,0)) },
                {"TopPlate",new IfMaterial(ifModel,new IfColor(255,102,255)) },
                {"BottomPlate",new IfMaterial(ifModel,new IfColor(0, 0, 255)) },
                {"RLeft",new IfMaterial(ifModel,IfColor.GetColor(IfColorEnum.DodgerBlue)) },
                {"RRight",new IfMaterial(ifModel,IfColor.GetColor(IfColorEnum.DarkGolden)) },
                {"RBetween",new IfMaterial(ifModel,IfColor.GetColor(IfColorEnum.Orange)) },
                {"TopStud",new IfMaterial(ifModel,IfColor.GetColor(IfColorEnum.LightSlateGrey)) },
                {"BottomStud",new IfMaterial(ifModel,IfColor.GetColor(IfColorEnum.DeepSkyBlue)) },
                {"Joist",new IfMaterial(ifModel,IfColor.GetColor(IfColorEnum.Violet)) },

            };

            IfStud.Setup = new Setup()
            {
                 {"Dimension",new IfDimension(2,6,2) }
            };

            IfSill.Setup = new Setup()
            {
                {"Dimension",new IfDimension(2,6,2) }
            };


        }


        static Startup()
        {

        }

    }
}
