

namespace Bim.Domain
{
    public interface IStory
    {
        int StoryNo { get; set; }
        IBuilding Building { get; set; }
    }
}
