using ISMNewsPortal.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISMNewsPortal.BLL.Tools
{
    public static class CommentsSort
    {
        public static IEnumerable<Comment> SortBy(IEnumerable<Comment> items, string sortType)
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
                case "commenttext":
                    items = items.OrderBy(u => u.Text);
                    break;
                default:
                    items = items.OrderBy(u => u.Date);
                    break;
            }
            return items;
        }

        public static IEnumerable<Comment> SortByReversed(IEnumerable<Comment> items, string sortType)
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
                case "commenttext":
                    items = items.OrderByDescending(u => u.Text);
                    break;
                default:
                    items = items.OrderByDescending(u => u.Date);
                    break;
            }
            return items;
        }

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
                case "commenttext":
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
                case "commenttext":
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
