using System.Collections.Generic;

namespace ISMNewsPortal.Models
{
    public class NewsPostSimplifiedCollection : ToolBarModel
    {
        public NewsPostSimplifiedCollection(ICollection<NewsPostSimplifiedView> newsPostSimplifiedViews, ToolBarModel toolBarModel) : base(toolBarModel)
        {
            NewsPostSimpliedViews = newsPostSimplifiedViews;
        }
        public ICollection<NewsPostSimplifiedView> NewsPostSimpliedViews { get; set; }
    }
}