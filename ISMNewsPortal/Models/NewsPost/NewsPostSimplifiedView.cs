using ISMNewsPortal.BLL.DTO;
using ISMNewsPortal.Helpers;
using System;
using System.ComponentModel.DataAnnotations;

namespace ISMNewsPortal.Models
{
    public class NewsPostSimplifiedView
    {
        public NewsPostSimplifiedView()
        {
        }
        public NewsPostSimplifiedView(NewsPostDTO newsPost, int commentsCount)
        {
            Id = newsPost.Id;
            Name = newsPost.Name;
            ImagePath = FileModelActions.GetNameByIdFormated(newsPost.ImageId);
            Description = newsPost.Description;
            CommentsCount = commentsCount;
            CreatedDate = newsPost.CreatedDate;
            PublicationDate = newsPost.PublicationDate;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImagePath { get; set; }
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        public int CommentsCount { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime PublicationDate { get; set; }
    }
}