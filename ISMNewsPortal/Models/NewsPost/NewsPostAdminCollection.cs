using ISMNewsPortal.BLL.BusinessModels;
using System.Collections.Generic;

namespace ISMNewsPortal.Models
{
    public class NewsPostAdminCollection : Options
    {
        public NewsPostAdminCollection(ICollection<NewsPostAdminView> newsPostAdminViews, Options toolBarModel) : base(toolBarModel)
        {
            NewsPostAdminViews = newsPostAdminViews;
        }
        public ICollection<NewsPostAdminView> NewsPostAdminViews { get; set; }
    }
}