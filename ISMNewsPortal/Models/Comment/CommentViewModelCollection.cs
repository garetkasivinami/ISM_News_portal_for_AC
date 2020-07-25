using ISMNewsPortal.BLL.DTO;
using System.Collections.Generic;

namespace ISMNewsPortal.Models
{

    public class CommentViewModelCollection : ToolBarModel
    {
        public CommentViewModelCollection(NewsPost newsPost, string imagePath, ICollection<CommentViewModel> commentViewModels, ToolBarModel toolBar, int commentsCount) : base(toolBar)
        {
            CommentViewModels = commentViewModels;
            CommentsCount = commentsCount;
            NewsPost = newsPost;
            ImagePath = imagePath;
        }
        public NewsPost NewsPost { get; set; }
        public string ImagePath { get; set; }
        public ICollection<CommentViewModel> CommentViewModels { get; set; }
        public int CommentsCount { get; set; }
    }
}