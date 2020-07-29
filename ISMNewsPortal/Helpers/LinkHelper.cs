using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ISMNewsPortal.BLL.BusinessModels;

namespace ISMNewsPortal.Helpers
{
    public static class LinkHelper
    {
        public static object CreatePageLinks(Options toolsModel, int pageOffset = 0)
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
        public static object CreateFilterLinks(Options toolsModel, string filter)
        {
            return new
            {
                @Page = 1,
                @SortType = toolsModel.SortType,
                @Filter = filter,
                @Search = toolsModel.Search,
                @Reversed = toolsModel.Reversed
            };
        }
        public static object CreateSortLinks(Options toolsModel, string sortType, bool reversed)
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