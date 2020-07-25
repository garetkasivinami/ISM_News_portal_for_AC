using ISMNewsPortal.BLL.Models;
using ISMNewsPortal.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ISMNewsPortal.Models
{
    public class NewsPostEditModel : NewsPostCreateModel
    {
        public int Id { get; set; }
        [Display(Name = "CreatedDate", ResourceType = typeof(Language.Language))]
        public DateTime CreatedDate { get; set; }
        [Display(Name = "Image path")]
        public string ImagePath { get; set; }

        public NewsPostEditModel()
        {

        }
        public NewsPostEditModel(NewsPost newsPost) : base(newsPost)
        {
            Id = newsPost.Id;
            CreatedDate = newsPost.CreatedDate;
            ImagePath = FileModelActions.GetNameByIdFormated(ImageId);
        }

        public override NewsPost ConvertToNewsPost()
        {
            var newsPost = base.ConvertToNewsPost();

            newsPost.Id = Id;
            newsPost.CreatedDate = CreatedDate;
            newsPost.EditDate = DateTime.Now.ToUniversalTime();

            return newsPost;
        }
    }
}