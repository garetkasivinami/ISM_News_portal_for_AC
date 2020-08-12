using ISMNewsPortal.BLL.Models;
using ISMNewsPortal.Helpers;
using System;
using System.ComponentModel.DataAnnotations;

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

        public override NewsPost PassToNewsPost(NewsPost newsPostArgument)
        {
            DateTime postCreatedDate = newsPostArgument.CreatedDate;

            var newsPost = base.PassToNewsPost(newsPostArgument);

            newsPostArgument.CreatedDate = postCreatedDate;

            newsPost.Id = Id;
            newsPost.EditDate = DateTime.Now.ToUniversalTime();

            return newsPost;
        }
    }
}