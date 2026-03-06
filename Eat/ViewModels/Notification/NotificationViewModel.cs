using Eat.Models;

namespace Eat.ViewModels.Notification
{
    public class NotificationViewModel
    {
        public List<StoryVote> Votes { get; set; }
        public List<Comment> Comments { get; set; }
    }
}
