using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ISMNewsPortal.Models
{
    public class NewsPostViewModel
    {
        public NewsPostViewModel(NewsPost newsPost, Users author, ICollection<CommentViewModel> comments)
        {
            NewsPost = newsPost;
            Author = author;
            Comments = comments;
        }
        public NewsPost NewsPost { get; set; }
        public Users Author { get; set; }
        public ICollection <CommentViewModel> Comments { get; set; }
    }
    public class CommentViewModel
    {
        public CommentViewModel(Comment comment, Users author, bool editable)
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
        public Users Author { get; set; }
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
}