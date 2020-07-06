using System.Collections.Generic;

namespace ISMNewsPortal.Models
{
    public class NewsPostSimplifiedCollection : ToolBarModel
    {
        public ICollection<NewsPostSimplifiedView> NewsPostSimpliedViews { get; set; }
    }
}