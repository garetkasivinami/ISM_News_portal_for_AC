using ISMNewsPortal.BLL.Models;
using System;

namespace ISMNewsPortal.Models
{
    public class CommentViewModel
    {
        public CommentViewModel(Comment comment)
        {
            Id = comment.Id;
            Date = comment.Date;
            Text = comment.Text;
            Author = comment.UserName;
            NewsPostId = comment.NewsPostId;
        }
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Text { get; set; }
        public string Author { get; set; }
        public int NewsPostId { get; set; }
    }
}
