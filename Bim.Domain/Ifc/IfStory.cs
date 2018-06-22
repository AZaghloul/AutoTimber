using Bim.Common.Measures;
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
        public Length StoryElevation { get; set; }
        public Length StoryHeight { get; set; }
        public List<IfWall> IfWalls { get; set; }
        public List<IfFloor> IfFloors { get; set; }
        public IfBuilding IfBuilding { get; set; }
        public IIfcBuildingStorey IfcStory
        {
            get { return _ifcBuildingStorey; }
            set { _ifcBuildingStorey = value; Initialize(); }
        }
        public IBuilding Building { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IfStory() : base(null)
        {

        }
        public IfStory(IfBuilding ifBuilding, IIfcBuildingStorey ifcStory) : base(ifBuilding.IfModel)
        {
            IfBuilding = ifBuilding;
            IfcStory = ifcStory;
            Initialize();
        }
        private void Initialize()
        {
            Guid = IfcStory.GlobalId;
            Label = IfcStory.EntityLabel;
            Description = IfcStory.Description;
            StoryElevation = Length.FromFeet((double)IfcStory.Elevation.Value.Value);
        }
        public void GetWalls()
        {
            if (IfWalls != null)
                return;
            else
                IfWalls = IfWall.GetWalls(this);

        }
        public void GetFloors()
        {
            if (IfFloors != null)
                return;
            else
                IfFloors = IfFloor.GetFloors(this);
        }


        public static List<IfStory> GetStories(IfBuilding ifBuilding)
        {
            var ifStories = new List<IfStory>();
            var ifcStories = ifBuilding.IfModel.IfcStore.Instances.OfType<IIfcBuildingStorey>();
            IfStory ifStory;
            int counter = 0;
            foreach (var story in ifcStories)
            {
                ifStory = new IfStory(ifBuilding, story);
                ifStory.StoryNo = counter;
                ifStories.Add(ifStory);
                counter++;
            }

            foreach (var story in ifStories)
            {
                if (story.StoryNo < ifStories.Count-1)
                    story.StoryHeight = ifStories[story.StoryNo + 1].StoryElevation - ifStories[story.StoryNo].StoryElevation;
                else story.StoryHeight = Length.FromFeet(0);
                story.GetWalls();
                story.GetFloors();
            }

            return ifStories;
        }
    }
}
