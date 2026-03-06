using Eat.ViewModels.StoryVMs;

public class ReaderChapterVM
{
    // Chapter
    public int Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public int Views { get; set; }
    public DateTime CreatedDate { get; set; }

    public string AuthorId { get; set; }
    public string AuthorName { get; set; }

    public List<RelatedStoryVM> RelatedStories { get; set; } = new();
    // Story
    public int StoryId { get; set; }
    public string StoryTitle { get; set; }
    public string CoverImage { get; set; }

    // Navigation
    public int? PreviousChapterId { get; set; }
    public int? NextChapterId { get; set; }

    // Vote sistemi
    public int Upvotes { get; set; }
    public int Downvotes { get; set; }
    public int Score { get; set; }
    public bool? UserVote { get; set; } // null = səs verməyib, true = upvote, false = downvote
    public string AuthorProfileImage { get; set; }
    // Comment
    public int CommentCount { get; set; }
    public List<CommentItemVM> Comments { get; set; } = new List<CommentItemVM>();
}

