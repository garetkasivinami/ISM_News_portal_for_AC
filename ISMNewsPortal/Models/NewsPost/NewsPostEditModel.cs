using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISMNewsPortal.Models
{
    public class NewsPostEditModel : NewsPostCreateModel
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ImagePath { get; set; }

        public NewsPostEditModel()
        {

        }
        public NewsPostEditModel(NewsPost newsPost) : base(newsPost)
        {
            Id = newsPost.Id;
            CreatedDate = newsPost.CreatedDate;
            ImagePath = FileModel.GetNameByIdFormated(ImageId);
        }
    }
}