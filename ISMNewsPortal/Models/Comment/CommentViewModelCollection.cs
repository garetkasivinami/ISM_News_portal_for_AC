using ISMNewsPortal.BLL.Models;
using ISMNewsPortal.Models.Tools;
using System.Collections.Generic;

namespace ISMNewsPortal.Models
{

    public class CommentViewModelCollection : CommentToolsModel
    {
        public CommentViewModelCollection(NewsPost newsPost, string imagePath, ICollection<CommentViewModel> commentViewModels, CommentToolsModel toolBar, int commentsCount) : base(toolBar)
        {
            CommentViewModels = commentViewModels;
            CommentsCount = commentsCount;
            NewsPost = newsPost;
            ImagePath = imagePath;
            Page++;
        }
        public NewsPost NewsPost { get; set; }
        public string ImagePath { get; set; }
        public ICollection<CommentViewModel> CommentViewModels { get; set; }
        public int CommentsCount { get; set; }
    }
}