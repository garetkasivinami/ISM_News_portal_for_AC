using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Web;

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
    public class NewsPostAdminCollection
    {
        public ICollection<NewsPostAdminView> NewsPostAdminViews { get; set; }
        public bool ViewActionLinks { get; set; }
    }
    public class NewsPostSimplifiedCollection
    {
        public ICollection<NewsPostSimplifiedView> NewsPostSimplifyViews { get; set; }
        public int currentPage;
        public int pages;
        public string filter;
        public string sortType;
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
        [MaxLength(256)]
        public string ImagePath { get; set; }
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
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImagePath { get; set; }
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        public int CommentsCount { get; set; }
    }
    public class NewsPostAdminView : NewsPostSimplifiedView
    {
        public NewsPostAdminView()
        {
        }
        public NewsPostAdminView(NewsPost newsPost, string authorName, int commentsCount) : base(newsPost, commentsCount)
        {
            CreatedDate = newsPost.CreatedDate;
            EditDate = newsPost.EditDate;
            AuthorId = newsPost.AuthorId;
            AuthorName = authorName;
        }
        public DateTime CreatedDate { get; set; }
        public DateTime? EditDate { get; set; }
        public bool ForRegistered { get; set; }
        public string AuthorName { get; set; }
        public int AuthorId { get; set; }
    }
}