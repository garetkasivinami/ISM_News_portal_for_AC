using System;

namespace ISMNewsPortal.Models
{
    public class NewsPostAdminView : NewsPostSimplifiedView
    {
        public NewsPostAdminView()
        {
        }
        public NewsPostAdminView(NewsPost newsPost, string authorName, int commentsCount) : base(newsPost, commentsCount)
        {
            EditDate = newsPost.EditDate;
            AuthorId = newsPost.AuthorId;
            AuthorName = authorName;
            IsVisible = newsPost.IsVisible;
        }
        public DateTime? EditDate { get; set; }
        public string AuthorName { get; set; }
        public int AuthorId { get; set; }
        public bool IsVisible { get; set; }
    }
}