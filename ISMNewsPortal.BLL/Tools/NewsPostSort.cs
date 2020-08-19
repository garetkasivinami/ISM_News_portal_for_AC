using ISMNewsPortal.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISMNewsPortal.BLL.Tools
{
    public static class NewsPostSort
    {
        public static IEnumerable<NewsPost> SortBy(IEnumerable<NewsPost> items, string sortType)
        {
            sortType = sortType?.ToLower();
            switch (sortType)
            {
                case "id":
                    items = items.OrderBy(u => u.Id);
                    break;
                case "name":
                    items = items.OrderBy(u => u.Name);
                    break;
                case "description":
                    items = items.OrderBy(u => u.Description);
                    break;
                case "editdate":
                    items = items.OrderBy(u => u.EditDate);
                    break;
                case "author":
                    items = items.OrderBy(u => u.AuthorId);
                    break;
                case "publicationdate":
                    items = items.OrderBy(u => u.PublicationDate);
                    break;
                case "visibility":
                    items = items.OrderBy(u => u.IsVisible);
                    break;
                default:
                    items = items.OrderBy(u => u.CreatedDate);
                    break;
            }
            return items;
        }

        public static IEnumerable<NewsPost> SortByReversed(IEnumerable<NewsPost> items, string sortType)
        {
            sortType = sortType?.ToLower();
            switch (sortType)
            {
                case "id":
                    items = items.OrderByDescending(u => u.Id);
                    break;
                case "name":
                    items = items.OrderByDescending(u => u.Name);
                    break;
                case "description":
                    items = items.OrderByDescending(u => u.Description);
                    break;
                case "editdate":
                    items = items.OrderByDescending(u => u.EditDate);
                    break;
                case "author":
                    items = items.OrderByDescending(u => u.AuthorId);
                    break;
                case "publicationdate":
                    items = items.OrderByDescending(u => u.PublicationDate);
                    break;
                case "visibility":
                    items = items.OrderByDescending(u => u.IsVisible);
                    break;
                default:
                    items = items.OrderByDescending(u => u.CreatedDate);
                    break;
            }
            return items;
        }
        public static IQueryable<NewsPost> SortBy(IQueryable<NewsPost> items, string sortType)
        {
            sortType = sortType?.ToLower();
            switch (sortType)
            {
                case "id":
                    items = items.OrderBy(u => u.Id);
                    break;
                case "name":
                    items = items.OrderBy(u => u.Name);
                    break;
                case "description":
                    items = items.OrderBy(u => u.Description);
                    break;
                case "editdate":
                    items = items.OrderBy(u => u.EditDate);
                    break;
                case "author":
                    items = items.OrderBy(u => u.AuthorId);
                    break;
                case "publicationdate":
                    items = items.OrderBy(u => u.PublicationDate);
                    break;
                case "visibility":
                    items = items.OrderBy(u => u.IsVisible);
                    break;
                default:
                    items = items.OrderBy(u => u.CreatedDate);
                    break;
            }
            return items;
        }

        public static IQueryable<NewsPost> SortByReversed(IQueryable<NewsPost> items, string sortType)
        {
            sortType = sortType?.ToLower();
            switch (sortType)
            {
                case "id":
                    items = items.OrderByDescending(u => u.Id);
                    break;
                case "name":
                    items = items.OrderByDescending(u => u.Name);
                    break;
                case "description":
                    items = items.OrderByDescending(u => u.Description);
                    break;
                case "editdate":
                    items = items.OrderByDescending(u => u.EditDate);
                    break;
                case "author":
                    items = items.OrderByDescending(u => u.AuthorId);
                    break;
                case "publicationdate":
                    items = items.OrderByDescending(u => u.PublicationDate);
                    break;
                case "visibility":
                    items = items.OrderByDescending(u => u.IsVisible);
                    break;
                default:
                    items = items.OrderByDescending(u => u.CreatedDate);
                    break;
            }
            return items;
        }
    }
}
