using ISMNewsPortal.BLL.BusinessModels;
using System.Collections.Generic;

namespace ISMNewsPortal.Models
{
    public class NewsPostSimplifiedCollection : Options
    {
        public NewsPostSimplifiedCollection(ICollection<NewsPostSimplifiedView> newsPostSimplifiedViews, Options toolBarModel) : base(toolBarModel)
        {
            NewsPostSimpliedViews = newsPostSimplifiedViews;
        }
        public ICollection<NewsPostSimplifiedView> NewsPostSimpliedViews { get; set; }
    }
}