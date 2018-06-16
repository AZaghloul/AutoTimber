using Bim.Application.IRCWood.IRC;
using Bim.Application.IRCWood.Physical;
using Bim.Common.Measures;
using Bim.Domain.General;
using Bim.Domain.Ifc;

using System.Threading.Tasks;

namespace Bim.Application.IRCWood.Common
{
    public class IfStartup
    {
      

        public IfStartup()
        {

        }
        #region AsyncMethods
       
        #endregion


        public void Configuration(IfModel ifModel)
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

        public void Configure(IfModel ifModel,WoodFrame woodFrame)
        {

            #region AsyncMethods
            //string filePath = @"..\..\Models\home-2floor-ft.ifc";
            //Task<IfModel> LoadModelAsync = Task.Factory.StartNew<IfModel>((filepath) =>
            //{
            //    //load Tables;
            //    return IfModel.Open(filePath);
            //}, filePath);

            Task<DesignOptions> LoadDesignOptionsAsync = Task.Factory.StartNew(() =>
           {
               //load settings from Database;
               return new DesignOptions();

           });

            Task LoadConfigurationAsync = Task.Factory.StartNew(() =>
           {
               //load configuration file from database or whatever;
           });

            Task<StudTable> LoadStudTablesAsync = Task.Factory.StartNew(() =>
           {
               //  load Tables;
               return StudTable.Load();
           });

            Task<Table502_3_1> LoadHeaderTablesAsync = Task.Factory.StartNew(() =>
            {
                //  load Tables;
                return Table502_3_1.Load(null);
            });
            //

            #endregion

            //load model design option and configuration
            ifModel.DesignOptions = LoadDesignOptionsAsync.Result;
            

            //load wood frame tables
            woodFrame.StudTable= LoadStudTablesAsync.Result;
            woodFrame.JoistTable = LoadHeaderTablesAsync.Result;

        }

       
    }
}
