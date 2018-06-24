using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xbim.Ifc;
using Xbim.Ifc4.GeometricConstraintResource;
using Xbim.Ifc4.GeometryResource;
using Xbim.Ifc4.Interfaces;
using Xbim.Ifc4.Kernel;
using Xbim.Ifc4.ProductExtension;

namespace Bim.Domain.Ifc
{
    public class IfBuilding : IfElement
    {
        public IIfcBuilding IfcBuilding { get; set; }
        public List<IfStory> IfStories { get; set; }
        public IModel Model { get; set; }
        #region Constructor
        public IfBuilding() : base(null)
        {
        }
        public IfBuilding(IfModel ifModel):base(ifModel)
        {
            Intialize();
        }
        #endregion

        #region HelperMethod
        private void Intialize()
        {
            if (IfStories != null) return;
            IfStories = new List<IfStory>();
            IfStories = IfStory.GetStories(this);

        }
        #endregion

        #region Methods
        public static IfBuilding New(IfModel ifModel, string name)
        {
            IfBuilding ifBuilding = new IfBuilding();
            var model = ifModel.IfcStore;
            using (var txn = model.BeginTransaction("Create Building"))
            {
                var building = model.Instances.New<IfcBuilding>();
                building.Name = name;
                building.CompositionType = IfcElementCompositionEnum.ELEMENT;
                var localPlacement = model.Instances.New<IfcLocalPlacement>();
                building.ObjectPlacement = localPlacement;
                var placement = model.Instances.New<IfcAxis2Placement3D>();
                localPlacement.RelativePlacement = placement;
                placement.Location = model.Instances.New<IfcCartesianPoint>(p => p.SetXYZ(0, 0, 0));
                var project = model.Instances.OfType<IfcProject>().FirstOrDefault();
                project?.AddBuilding(building);
                txn.Commit();
                //set the Ifcbuilding
                ifBuilding.IfcBuilding = building;
            }
            return ifBuilding;
        }

        public static List<IfBuilding> GetBuildings(IfModel ifModel)
        {
            List<IfBuilding> ifBuildings = new List<IfBuilding>();
            IfBuilding ifBuidling;
            foreach (var building in ifModel.IfcStore.Instances.OfType<IIfcBuilding>())
            {
                ifBuidling = new IfBuilding(ifModel)
                {
                    IfcBuilding = building,
                    IfModel = ifModel
                };
                ifBuildings.Add(ifBuidling);
            }

            return ifBuildings;
        }
        #endregion
    }
}
