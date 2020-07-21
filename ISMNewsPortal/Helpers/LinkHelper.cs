using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISMNewsPortal.Helpers
{
    public static class LinkHelper
    {
        public static object CreatePageLinks(ISMNewsPortal.Models.ToolBarModel toolsModel, int pageOffset = 0)
        {
            return new
            {
                @Page = toolsModel.Page + pageOffset,
                @SortType = toolsModel.SortType,
                @Filter = toolsModel.Filter,
                @Search = toolsModel.Search,
                @Reversed = toolsModel.Reversed
            };
        }
        public static object CreateFilterLinks(ISMNewsPortal.Models.ToolBarModel toolsModel, string filter)
        {
            return new
            {
                @Page = toolsModel.Page,
                @SortType = toolsModel.SortType,
                @Filter = filter,
                @Search = toolsModel.Search,
                @Reversed = toolsModel.Reversed
            };
        }
        public static object CreateSortLinks(ISMNewsPortal.Models.ToolBarModel toolsModel, string sortType, bool reversed)
        {
            return new
            {
                @Page = toolsModel.Page,
                @SortType = sortType,
                @Filter = toolsModel.Filter,
                @Search = toolsModel.Search,
                @Reversed = reversed
            };
        }
        public static string GetLocalizedByName(string field)
        {
            return field == null ? null : Language.Language.ResourceManager.GetString(field, Language.Language.Culture);
        }
    }
}