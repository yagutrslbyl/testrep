namespace Eat.Models
{
    public class UserFollow
    {
        public int Id { get; set; }

        public string FollowerId { get; set; } // Takip eden
        public AppUser Follower { get; set; }

        public string FollowingId { get; set; } // Takip edilen
        public AppUser Following { get; set; }

        public DateTime FollowedAt { get; set; } = DateTime.Now;
    }
}
