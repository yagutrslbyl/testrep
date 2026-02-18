namespace Eat.Models
{
    public class Comment
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Foreign Keys
        public int StoryId { get; set; }
        public Story Story { get; set; }

        public int UserId { get; set; }
        public AppUser User { get; set; }
    }
}
