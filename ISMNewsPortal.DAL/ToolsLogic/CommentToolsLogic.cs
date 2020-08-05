using ISMNewsPortal.BLL.Models;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISMNewsPortal.DAL.ToolsLogic
{
    public static class CommentToolsLogic
    {
        public static IQueryable<Comment> SortBy(IQueryable<Comment> items, string sortType)
        {
            sortType = sortType?.ToLower();
            switch (sortType)
            {
                case "id":
                    items = items.OrderBy(u => u.Id);
                    break;
                case "username":
                    items = items.OrderBy(u => u.UserName);
                    break;
                case "text":
                    items = items.OrderBy(u => u.Text);
                    break;
                default:
                    items = items.OrderBy(u => u.Date);
                    break;
            }
            return items;
        }

        public static IQueryable<Comment> SortByReversed(IQueryable<Comment> items, string sortType)
        {
            sortType = sortType?.ToLower();
            switch (sortType)
            {
                case "id":
                    items = items.OrderByDescending(u => u.Id);
                    break;
                case "username":
                    items = items.OrderByDescending(u => u.UserName);
                    break;
                case "text":
                    items = items.OrderByDescending(u => u.Text);
                    break;
                default:
                    items = items.OrderByDescending(u => u.Date);
                    break;
            }
            return items;
        }
    }
}
