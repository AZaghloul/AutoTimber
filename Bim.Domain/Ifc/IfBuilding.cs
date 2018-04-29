using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xbim.Ifc4.Interfaces;

namespace Bim.Domain.Ifc
{
    public class IfBuilding : IElement
    {
        public int Id { get; set; }
        public string Label { get; set; }
        public IfModel IfModel { get; set; }
        public IIfcBuilding IfcBuilding { get; set; }
        List<IfStory> Stories { get; set; }

        public IfBuilding()
        {

            Stories = new List<IfStory>();
            Inialize();
        }

      private   void Inialize()
        {
            Label = IfcBuilding.Name;
            Id = (int)IfcBuilding.GlobalId.Value;

            var stories = IfcBuilding.IsDecomposedBy.OfType<IIfcBuildingStorey>();
            IfStory ifStory;
            int counter = 0;
            foreach (var story in stories)
            {

                ifStory = new IfStory();
                ifStory.IfModel = IfModel;
                ifStory.Id = (int)story.GlobalId.Value;
                ifStory.Label = story.Name;
                ifStory.StoryNo = counter;
                Stories.Add(ifStory);
                counter++;
            }
        }
    }
}
