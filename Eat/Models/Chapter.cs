namespace Eat.Models
{
    public class Chapter
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Foreign Key
        public int StoryId { get; set; }

        public Story Story { get; set; }
    }
}
