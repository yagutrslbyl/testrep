namespace Eat.ViewModels.StoryVMs
{
    public class ReaderStoryDetailsVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string CoverImageUrl { get; set; }
        public string Description { get; set; }
        public string AuthorName { get; set; }
        public string Language { get; set; }
        public List<string> Tags { get; set; }
        public int TotalViews { get; set; }
        public int Votes { get; set; }
        public List<ReaderChapterVM> Chapters { get; set; }
        public List<RelatedStoryVM> RelatedStories { get; set; }
    }
}
