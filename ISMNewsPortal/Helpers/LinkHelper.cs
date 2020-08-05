using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ISMNewsPortal.BLL.BusinessModels;
using ISMNewsPortal.Models;
using ISMNewsPortal.Models.Tools;

namespace ISMNewsPortal.Helpers
{
    public static class LinkHelper
    {
        public static object CreatePageLinks(ToolsModel toolsModel, int pageOffset = 0)
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
        public static object CreateFilterLinks(ToolsModel toolsModel, string filter)
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
        public static object CreateSortLinks(ToolsModel toolsModel, string sortType, bool reversed)
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

        public static object CreateCommentPageLinks(CommentToolsModel toolsModel, int pageOffset = 0)
        {
            return new
            {
                @Page = toolsModel.Page + pageOffset,
                @SortType = toolsModel.SortType,
                @Filter = toolsModel.Filter,
                @Search = toolsModel.Search,
                @Reversed = toolsModel.Reversed,
                @id = toolsModel.Id
            };
        }
        public static object CreateCommentFilterLinks(CommentToolsModel toolsModel, string filter)
        {
            return new
            {
                @Page = 1,
                @SortType = toolsModel.SortType,
                @Filter = filter,
                @Search = toolsModel.Search,
                @Reversed = toolsModel.Reversed,
                @id = toolsModel.Id
            };
        }
        public static object CreateCommentSortLinks(CommentToolsModel toolsModel, string sortType, bool reversed)
        {
            return new
            {
                @Page = toolsModel.Page,
                @SortType = sortType,
                @Filter = toolsModel.Filter,
                @Search = toolsModel.Search,
                @Reversed = reversed,
                @id = toolsModel.Id
            };
        }

        public static string GetLocalizedByName(string field)
        {
            return field == null ? null : Language.Language.ResourceManager.GetString(field, Language.Language.Culture);
        }
    }
}