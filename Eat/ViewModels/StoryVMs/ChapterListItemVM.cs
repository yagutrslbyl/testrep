namespace Eat.ViewModels.StoryVMs
{
    public class ChapterListItemVM
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public DateTime CreatedDate { get; set; }

        // İleride eklemek için hazır alanlar
        public int Views { get; set; }
        public int Votes { get; set; }
        public int CommentsCount { get; set; }
    }
}
