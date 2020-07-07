using System.Collections.Generic;

namespace ISMNewsPortal.Models
{
    public class NewsPostAdminCollection : ToolBarModel
    {
        public NewsPostAdminCollection(ICollection<NewsPostAdminView> newsPostAdminViews, ToolBarModel toolBarModel) : base(toolBarModel)
        {
            NewsPostAdminViews = newsPostAdminViews;
        }
        public ICollection<NewsPostAdminView> NewsPostAdminViews { get; set; }
    }
}