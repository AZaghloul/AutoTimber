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

        public IfBuilding(IIfcBuilding ifcBuilding)
        {
            Stories = new List<IfStory>();
            IfcBuilding = ifcBuilding;
            Intialize();
        }

        private void Intialize()
        {
            Label = IfcBuilding.Name;
            //Id = (int)IfcBuilding.GlobalId.Value;

            var stories = IfcBuilding.IsDecomposedBy.OfType<IIfcBuildingStorey>();
            IfStory ifStory;
            int counter = 0;
            foreach (var story in stories)
            {
                ifStory = new IfStory
                {
                    IfModel = IfModel,
                    Id = (int)story.GlobalId.Value,
                    Label = story.Name,
                    StoryNo = counter
                };
                Stories.Add(ifStory);
                counter++;
            }
        }
    }
}
