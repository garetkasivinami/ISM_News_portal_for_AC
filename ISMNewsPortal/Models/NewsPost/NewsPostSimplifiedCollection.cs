using ISMNewsPortal.BLL.BusinessModels;
using System.Collections.Generic;

namespace ISMNewsPortal.Models
{
    public class NewsPostSimplifiedCollection : ToolsModel
    {
        public NewsPostSimplifiedCollection(ICollection<NewsPostSimplifiedView> newsPostSimplifiedViews, ToolsModel toolBarModel) : base(toolBarModel)
        {
            NewsPostSimpliedViews = newsPostSimplifiedViews;
            Page++;
        }
        public ICollection<NewsPostSimplifiedView> NewsPostSimpliedViews { get; set; }
    }
}