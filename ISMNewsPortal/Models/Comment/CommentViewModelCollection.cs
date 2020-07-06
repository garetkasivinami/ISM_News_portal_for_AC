namespace ISMNewsPortal.Models
{
    using System.Collections.Generic;

    public class CommentViewModelCollection : ToolBarModel
    {
        public ICollection<CommentViewModel> CommentViewModels { get; set; }
        public int CommentsCount { get; set; }
    }
}
