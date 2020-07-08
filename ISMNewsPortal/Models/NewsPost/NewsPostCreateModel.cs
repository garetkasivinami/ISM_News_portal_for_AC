using System;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace ISMNewsPortal.Models
{
    public class NewsPostCreateModel
    {
        [Required]
        [MaxLength(128)]
        public string Name { get; set; }
        [Required]
        [MaxLength(10000)]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        public HttpPostedFileBase[] uploadFiles { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime? PublicationDate { get; set; }
        public bool IsVisible { get; set; }
        public int AuthorId { get; set; }
        public int ImageId { get; set; }

        public NewsPostCreateModel()
        {

        }
        public NewsPostCreateModel(NewsPost newsPost)
        {
            Name = newsPost.Name;
            Description = newsPost.Description;
            PublicationDate = newsPost.PublicationDate;
            IsVisible = newsPost.IsVisible;
            AuthorId = newsPost.AuthorId;
            ImageId = newsPost.ImageId;
        }
    }
}