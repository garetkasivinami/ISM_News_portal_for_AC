using ISMNewsPortal.BLL.DTO;
using ISMNewsPortal.DAL.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace ISMNewsPortal.Models
{
    public class CommentCreateModel
    {
        [Required]
        public int PageId { get; set; }
        [Required]
        [MaxLength(100)]
        [Display(Name = "Username", ResourceType = typeof(Language.Language))]
        public string UserName { get; set; }
        [DataType(DataType.MultilineText)]
        [Required]
        [Display(Name = "CommentText", ResourceType = typeof(Language.Language))]
        public string Text { get; set; }

        public Comment ConvertToComment()
        {
            var comment = new Comment();

            comment.UserName = UserName;
            comment.Date = DateTime.Now;
            comment.Text = Text;
            comment.NewsPostId = PageId;

            return comment;
        }
    }
}
