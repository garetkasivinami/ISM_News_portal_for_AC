using ISMNewsPortal.BLL.BusinessModels;
using ISMNewsPortal.Models.Tools;
using System.Collections.Generic;

namespace ISMNewsPortal.Models
{
    public class NewsPostAdminCollection : ToolsModel
    {
        public NewsPostAdminCollection(ICollection<NewsPostAdminView> newsPostAdminViews, ToolsModel toolBarModel) : base(toolBarModel)
        {
            NewsPostAdminViews = newsPostAdminViews;
            Page++;
        }
        public ICollection<NewsPostAdminView> NewsPostAdminViews { get; set; }
    }
}