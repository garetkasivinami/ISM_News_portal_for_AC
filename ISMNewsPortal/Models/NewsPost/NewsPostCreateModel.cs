using ISMNewsPortal.BLL.DTO;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

namespace ISMNewsPortal.Models
{
    public class NewsPostCreateModel
    {
        [Required]
        [Display(Name = "NewsPostName", ResourceType = typeof(Language.Language))]
        public string Name { get; set; }
        [Required]
        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Description", ResourceType = typeof(Language.Language))]
        public string Description { get; set; }
        public HttpPostedFileBase[] uploadFiles { get; set; }
        [DataType(DataType.DateTime)]
        [Display(Name = "PublicationDate", ResourceType = typeof(Language.Language))]
        public DateTime? PublicationDate { get; set; }
        [Display(Name = "Visibility", ResourceType = typeof(Language.Language))]
        public bool IsVisible { get; set; }
        [Display(Name = "Author Id")]
        public int AuthorId { get; set; }
        [Display(Name = "Image Id")]
        public int ImageId { get; set; }
        public int? MinutesOffset { get; set; }

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

        public virtual NewsPost ConvertToNewsPost()
        {
            var newsPost = new NewsPost();

            newsPost.Name = Name;
            newsPost.Description = Description;
            newsPost.CreatedDate = DateTime.Now.ToUniversalTime();
            newsPost.EditDate = null;
            newsPost.ImageId = ImageId;
            newsPost.AuthorId = AuthorId;
            newsPost.IsVisible = IsVisible;

            newsPost.PublicationDate = PublicationDate?.AddMinutes(MinutesOffset ?? 0) ?? DateTime.Now.ToUniversalTime();
            return newsPost;
        }
    }
}