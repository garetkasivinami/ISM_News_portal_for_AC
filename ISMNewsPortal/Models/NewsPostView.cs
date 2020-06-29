using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public ICollection <CommentViewModel> Comments { get; set; }
        public bool IsLiked { get; set; }
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