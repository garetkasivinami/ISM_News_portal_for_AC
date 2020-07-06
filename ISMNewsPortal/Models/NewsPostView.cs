using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI.HtmlControls;

namespace ISMNewsPortal.Models
{
    public class NewsPostViewModel
    {
        public NewsPostViewModel(NewsPost newsPost, ICollection<CommentViewModel> comments)
        {
            NewsPost = newsPost;
            Comments = comments;
        }
        public NewsPost NewsPost { get; set; }
        public ICollection<CommentViewModel> Comments { get; set; }
    }
    public class NewsPostAdminCollection : ToolBarModel
    {
        public ICollection<NewsPostAdminView> NewsPostAdminViews { get; set; }
        public bool ViewActionLinks { get; set; }
    }
    public class NewsPostSimplifiedCollection : ToolBarModel
    {
        public ICollection<NewsPostSimplifiedView> NewsPostSimpliedViews { get; set; }
    }
    public class NewsPostModelCreate
    {
        [Required]
        [MaxLength(128)]
        public string Name { get; set; }
        [Required]
        [MaxLength(10000)]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        [Required]
        public HttpPostedFileBase[] uploadFiles { get; set; }
    }
    public class NewsPostSimplifiedView
    {
        public NewsPostSimplifiedView()
        {
        }
        public NewsPostSimplifiedView(NewsPost newsPost, int commentsCount)
        {
            Id = newsPost.Id;
            Name = newsPost.Name;
            ImagePath = newsPost.ImagePath;
            Description = newsPost.Description;
            CommentsCount = commentsCount;
            CreatedDate = newsPost.CreatedDate;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImagePath { get; set; }
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        public int CommentsCount { get; set; }
        public DateTime CreatedDate { get; set; }
    }
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
        }
        public DateTime? EditDate { get; set; }
        public string AuthorName { get; set; }
        public int AuthorId { get; set; }
    }
}