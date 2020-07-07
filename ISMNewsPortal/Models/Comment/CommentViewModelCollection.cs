namespace ISMNewsPortal.Models
{
    using System.Collections.Generic;

    public class CommentViewModelCollection : ToolBarModel
    {
        public CommentViewModelCollection(ICollection<CommentViewModel> commentViewModels, ToolBarModel toolBar, int commentsCount) : base(toolBar)
        {
            CommentViewModels = commentViewModels;
            CommentsCount = commentsCount;
        }
        public ICollection<CommentViewModel> CommentViewModels { get; set; }
        public int CommentsCount { get; set; }
    }
}
