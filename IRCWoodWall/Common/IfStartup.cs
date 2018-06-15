using Bim.Application.IRCWood.IRC;
using Bim.Common.Measures;
using Bim.Domain.General;
using Bim.Domain.Ifc;
using IRCWoodWall.Common;
using System.Threading.Tasks;

namespace Bim.Application.IRCWood.Common
{
    public class IfStartup
    {
        #region Properties
        public static StudTable Studtable { get; set; }
        public static DesignOptions DesignOptions { get; set; }
        #endregion
        
        #region AsyncMethods
        public static Task LoadDesignOptionsAsync = Task.Factory.StartNew(() =>
         {
             //load settings from Database;
             DesignOptions = new DesignOptions();

         });

        public static Task LoadConfigurationAsync = Task.Factory.StartNew(() =>
        {
            //load configuration file from database or whatever;
        });

        //public static Task<StudTable> LoadTablesAsync = Task.Factory.StartNew(() =>
        //{
        //    //load Tables;
        // return  // Studtable = StudTable.Load(" ");
        //});
        public static Task LoadFileAsync = Task.Factory.StartNew(() =>
         {
             //load Tables;
         });
        #endregion
        public static void Configuration(IfModel ifModel)
        {
            //configure the Length class
            Length.RoundValue = 4;
            //Configure Material Class
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

            };
            //Configure Sill Class
            IfStud.Setup = new Setup()
            {
                 {"Dimension",new IfDimension(2,6,2) }
            };
            //Configure Sill Class 
            IfSill.Setup = new Setup()
            {
                {"Dimension",new IfDimension(2,6,2) }
            };

        }


        static IfStartup()
        {

        }

    }
}
