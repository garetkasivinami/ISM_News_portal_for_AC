using System;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace ISMNewsPortal.Models
{
    public class NewsPostCreateModel
    {
        [Required]
        [MaxLength(128)]
        [Display(Name = "Title")]
        public string Name { get; set; }
        [Required]
        [MaxLength(10000)]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Description")]
        public string Description { get; set; }
        public HttpPostedFileBase[] uploadFiles { get; set; }
        [DataType(DataType.DateTime)]
        [Display(Name = "Publication date (UTC+0)")]
        public DateTime? PublicationDate { get; set; }
        [Display(Name = "Is visible")]
        public bool IsVisible { get; set; }
        [Display(Name = "Author Id")]
        public int AuthorId { get; set; }
        [Display(Name = "Image Id")]
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