namespace Eat.Models
{
    public class StoryVote
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public AppUser User { get; set; }

        public int StoryId { get; set; }
        public Story Story { get; set; }

        public bool Upvote { get; set; } // True = upvote, false = downvote
    }
}
