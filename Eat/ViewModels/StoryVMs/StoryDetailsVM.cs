using Eat.Models;

namespace Eat.ViewModels.StoryVMs
{
    public class StoryDetailsVM
    {
        public Story Story { get; set; }
        public List<Chapter> Chapters { get; set; }
        public List<Comment> Comments { get; set; }
    }
}
