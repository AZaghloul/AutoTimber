using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xbim.Ifc4.Interfaces;

namespace Bim.Domain.Ifc
{
    public class IfStory : IfElement, IStory
    {
        IIfcBuildingStorey _ifcBuildingStorey;
        public int StoryNo { get; set; }
        public List<IfWall> IfWalls { get; set; }
        public IfBuilding IfBuilding { get; set; }
        public IIfcBuildingStorey IfcStory
        {
            get { return _ifcBuildingStorey; }
            set { _ifcBuildingStorey = value; Initialize(); }
        }
        public IBuilding Building { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IfStory()
        {

        }
        public IfStory(IfModel ifModel, IIfcBuildingStorey ifcStory)
        {
            IfModel = ifModel;
            IfcStory = ifcStory;
            Initialize();
            IfModel.StoriesCollection.Add(this);
<<<<<<< HEAD
<<<<<<< HEAD

=======
>>>>>>> parent of c0e791d... Merge branch 'BOQ' into Algorithm-MVC
=======
>>>>>>> parent of c0e791d... Merge branch 'BOQ' into Algorithm-MVC
        }
        private void Initialize()
        {
            Guid = IfcStory.GlobalId;
            Label = IfcStory.EntityLabel;
            Description = IfcStory.Description;

            if (IfWalls != null) return;

            IfWalls = IfWall.GetWalls(this);
        }

        public static List<IfStory> GetStories(IfBuilding ifBuilding)
        {
            var ifStories = new List<IfStory>();
            var ifcStories = ifBuilding.IfModel.IfcStore.Instances.OfType<IIfcBuildingStorey>();
            IfStory ifStory;
            int counter = ifcStories.Count();

            //state of stories
            #region Updat Model State
            if (counter < 1)
            {
                ifBuilding.IfModel.State.Warrnings("There is No Stories found!", "Re export the ifc file again");
            }
            else if (counter >= 1)
            {
                ifBuilding.IfModel.State.HasBuilding = true;
                ifBuilding.IfModel.State.Passed($"{counter} Stories(s) found!", " ");
            }
            #endregion

            foreach (var story in ifcStories)
            {
                ifStory = new IfStory(ifBuilding.IfModel, story);
                ifStory.StoryNo = counter;
                ifStories.Add(ifStory);
                counter--;
            }

            return ifStories;
        }
    }
}
