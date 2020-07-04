namespace ISMNewsPortal.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Comment
    {
        public const int CommentsInOnePage = 10;
        public virtual int Id { get; set; }
        public virtual string UserName { get; set; }
        public virtual DateTime Date { get; set; }
        public virtual string Text { get; set; }
        public virtual int NewsPostId { get; set; }
    }
    public class CommentViewModel
    {
        public CommentViewModel(Comment comment)
        {
            Id = comment.Id;
            Date = comment.Date;
            Text = comment.Text;
            Author = comment.UserName;
        }
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Text { get; set; }
        public string Author { get; set; }
    }
    public class CommentViewModelCollection
    {
        public ICollection<CommentViewModel> CommentViewModels { get; set; }
        public int Pages { get; set; }
        public int Page { get; set; }
    }
    public class CommentCreateModel
    {
        [Required]
        public int PageId { get; set; }
        [Required]
        [MaxLength(128)]
        public string UserName { get; set; }
        [DataType(DataType.MultilineText)]
        [Required]
        [MaxLength(1000)]
        public string Text { get; set; }
    }
}
