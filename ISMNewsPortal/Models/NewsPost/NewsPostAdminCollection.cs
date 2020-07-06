using System.Collections.Generic;

namespace ISMNewsPortal.Models
{
    public class NewsPostAdminCollection : ToolBarModel
    {
        public ICollection<NewsPostAdminView> NewsPostAdminViews { get; set; }
        public bool ViewActionLinks { get; set; }
    }
}