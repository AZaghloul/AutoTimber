using System.Collections.Generic;
using System.Linq;
using Xbim.Ifc;
using Xbim.Ifc4.Kernel;
using Xbim.Common.Step21;
using Xbim.Common;
using Bim.IO;


namespace Bim.Domain.Ifc
{
    /// <summary>
    /// This class imports, inzialize and Handle  the model 
    /// </summary>
    public class IfModel : IModel
    {
        private static IfcHandler ifcHandler = new IfcHandler();
        public IfcStore IfcStore { get; set; }
        public List<IObject> Instances { get; set; }
        public IVersion Version { get; set; }
        public List<IfBuilding> IfBuildings { get; set; }
        public IfUnit IfUnit { get; set; }
        #region Constructors
        public IfModel(IfcStore ifcStore)
        {
            IfcStore = ifcStore;
            Instances = new List<IObject>();
            Intialize();
        }
        public IfModel()
        {

        }

        #endregion
        #region Methods


        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath">fila path to save the ifc file</param>
        /// <param name="open">open the ifc file in default program</param>
        public void Save(string filePath = "Untitled")
        {
            IfcStore.SaveAs(filePath);
        }

        
        #endregion

        #region Static Functions
        public static IfModel Open(string filePath)
        {
            var Ifcmodel = ifcHandler.Open(filePath);

            return new IfModel(Ifcmodel);
        }
        public static IfModel New(string projectName, string buildingName, bool save, string filepath = "Untitled")
        {
            //first we need to set up some credentials for ownership of data in the new model
            var credentials = new XbimEditorCredentials
            {

                ApplicationDevelopersName = "ITI ",
                ApplicationFullName = " ",
                ApplicationIdentifier = " ",
                ApplicationVersion = "1.0",
                EditorsFamilyName = "ITI ",
                EditorsGivenName = "ITI ",
                EditorsOrganisationName = " ITI"
            };
            //now we can create an IfcStore, it is in Ifc4 format and will be held in memory rather than in a database
            //database is normally better in performance terms if the model is large >50MB of Ifc or if robust transactions are required

            var model = IfcStore.Create(credentials, IfcSchemaVersion.Ifc4, XbimStoreType.InMemoryModel);
            IfModel ifModel = new IfModel(model);
            //Begin a transaction as all changes to a model are ACID
            using (var txn = model.BeginTransaction("Initialise Model"))
            {

                //create a project
                var project = model.Instances.New<IfcProject>();

                //set the units to SI (mm and metres)
                project.Initialize(ProjectUnits.SIUnitsUK);
                project.Name = projectName;
                //now commit the changes, else they will be rolled back at the end of the scope of the using statement
                txn.Commit();
            }
            if (save)
            {
                model.SaveAs(filepath);
            }

            ifModel.IfBuildings.Add(IfBuilding.New(ifModel, buildingName));

            return ifModel;

        }

        #endregion

        #region Helper Function
        private void Intialize()
        {
            if (IfBuildings != null) return; //check if there is already buildings return
            IfBuildings = IfBuilding.GetBuildings(this);
            Version = new IfVersion(this);
            IfUnit = new IfUnit(this);
        }

        
        public void Delete<T>() where T : IPersistEntity
        {
            using (var txn = IfcStore.BeginTransaction("Deleting"))
            {
                var columns = IfcStore.Instances.OfType<T>();
                int c = columns.Count();
                for (int i = 0; i < c; i++)
                {
                    IfcStore.Delete(columns.FirstOrDefault());
                }

                txn.Commit();

            }
        }
        #endregion




    }

}
