using ISMNewsPortal.BLL.BusinessModels;
using ISMNewsPortal.BLL.Models;
using System.Collections.Generic;

namespace ISMNewsPortal.Models
{

    public class CommentViewModelCollection : ToolsModel
    {
        public CommentViewModelCollection(NewsPost newsPost, string imagePath, ICollection<CommentViewModel> commentViewModels, ToolsModel toolBar, int commentsCount) : base(toolBar)
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