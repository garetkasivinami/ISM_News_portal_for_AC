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
        public NewsPostViewModel(NewsPost newsPost, AuthorInfo author, ICollection<CommentViewModel> comments, bool isLiked)
        {
            NewsPost = newsPost;
            Author = author;
            Comments = comments;
            IsLiked = isLiked;
        }
        public NewsPost NewsPost { get; set; }
        public AuthorInfo Author { get; set; }
        public ICollection<CommentViewModel> Comments { get; set; }
        public bool IsLiked { get; set; }
    }
    public class NewsPostAdminCollection
    {
        public ICollection<NewsPostAdminView> NewsPostAdminViews { get; set; }
        public bool ViewActionLinks { get; set; }
    }
    public class NewsPostModelCreate
    {
        [Required]
        [MaxLength(128)]
        public string Name { get; set; }
        [Required]
        [MaxLength(10000)]
        [DataType(DataType.MultilineText)]
        public string Desc { get; set; }
        [Required]
        [MaxLength(256)]
        public string ImagePath { get; set; }
        public bool ForRegistered { get; set; }
    }
    public class NewsPostSimplifyView
    {
        public NewsPostSimplifyView()
        {

        }
        public NewsPostSimplifyView(NewsPost newsPost)
        {
            Id = newsPost.Id;
            Name = newsPost.Name;
            ImagePath = newsPost.ImagePath;
            Description = newsPost.Descrition;
            LikesCount = newsPost.LikesCount;
            CommentsCount = newsPost.CommentsCount;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImagePath { get; set; }
        public string Description { get; set; }
        public int LikesCount { get; set; }
        public int CommentsCount { get; set; }
    }
    public class NewsPostAdminView : NewsPostSimplifyView
    {
        public NewsPostAdminView()
        {

        }
        public NewsPostAdminView(NewsPost newsPost) : base(newsPost)
        {
            CreatedDate = newsPost.CreatedDate;
            EditDate = newsPost.EditDate;
            ForRegistered = newsPost.ForRegistered;
            AuthorId = newsPost.Author.Id;
            AuthorName = newsPost.Author.UserName;
        }
        public string Descrition { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? EditDate { get; set; }
        public bool ForRegistered { get; set; }
        public string AuthorName { get; set; }
        public int AuthorId { get; set; }
    }
    public class AuthorInfo
    {
        public int UserId;
        public string UserName;
    }
    public class CommentViewModel
    {
        public CommentViewModel(Comment comment, AuthorInfo author, bool editable)
        {
            Id = comment.Id;
            Date = comment.Date;
            IsEdited = comment.IsEdited;
            Text = comment.Text;
            Author = author;
            Editable = editable;
        }
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public bool IsEdited { get; set; }
        public string Text { get; set; }
        public AuthorInfo Author { get; set; }
        public bool Editable { get; set; }
    }
    public class CommentModel
    {
        public int PageId { get; set; }
        [DataType(DataType.MultilineText)]
        [Required]
        [MaxLength(1000)]
        public string Text { get; set; }
    }
    public class CommentEditModel
    {
        public int Id { get; set; }
        [DataType(DataType.MultilineText)]
        [Required]
        [MaxLength(1000)]
        public string Text { get; set; }
    }
}