using ISMNewsPortal.BLL.Models;
using ISMNewsPortal.Helpers;
using System.Collections.Generic;

namespace ISMNewsPortal.Models
{
    public class NewsPostViewModel
    {
        public NewsPostViewModel(NewsPost newsPost, ICollection<CommentViewModel> comments, int page, int pages, bool actionLinks, int commentsCount)
        {
            NewsPost = newsPost;
            Comments = comments;
            Page = page;
            Pages = pages;
            ImagePath = FileModelActions.GetNameByIdFormated(newsPost.ImageId);
            ActionLinks = actionLinks;
            CommentsCount = commentsCount;
        }
        public NewsPost NewsPost { get; set; }
        public string ImagePath { get; set; }
        public ICollection<CommentViewModel> Comments { get; set; }
        public int Page { get; set; }
        public int Pages { get; set; }
        public int CommentsCount { get; set; }
        public bool ActionLinks { get; set; }
    }
}