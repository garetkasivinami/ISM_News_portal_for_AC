namespace ISMNewsPortal.Models
{
    using NHibernate;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class NewsPostHelperActions
    {
        public static int CalculatePages(int count, int multiplier)
        {
            int pages = count / multiplier;
            if (count % multiplier != 0)
                pages++;
            return pages;
        }
        public static IEnumerable<T> CutIEnumarable<T>(IEnumerable<T> target, int startIndex, int count)
        {
            return target.Skip(startIndex).Take(count);
        }
        public static IEnumerable<T> CutIEnumarable<T>(int page, int multiplier, IEnumerable<T> target)
        {
            return CutIEnumarable(target, page * multiplier, multiplier);
        }
        public static string FilterToday()
        {
            return $"PublicationDate >= CONVERT(DATETIME, '{DateTime.Now.Year}.{DateTime.Now.Month}.{DateTime.Now.Day}')";
        }
        public static string FilterYesterday()
        {
            DateTime yesterday = DateTime.Now.AddDays(-1);
            return $"PublicationDate >= CONVERT(DATETIME, '{yesterday.Year}.{yesterday.Month}.{yesterday.Day}') AND CreatedDate < CONVERT(DATETIME, '{DateTime.Now.Year}.{DateTime.Now.Month}.{DateTime.Now.Day}')";
        }
        public static string FilterWeek()
        {
            DateTime week = DateTime.Now.AddDays(-7);
            return $"PublicationDate >= CONVERT(DATETIME, '{week.Year}.{week.Month}.{week.Day}')";
        }
        public static string FilterAll()
        {
            return "1 = 1";
        }
        public static string GetFilterSqlString(ref string filter)
        {
            string filterFunc;
            switch (filter)
            {
                case "today":
                    filterFunc = NewsPostHelperActions.FilterToday();
                    break;
                case "yesterday":
                    filterFunc = NewsPostHelperActions.FilterYesterday();
                    break;
                case "week":
                    filterFunc = NewsPostHelperActions.FilterWeek();
                    break;
                default:
                    filter = null;
                    filterFunc = NewsPostHelperActions.FilterAll();
                    break;
            }
            return filterFunc;
        }
        public static string GetSortSqlString(ref string sortType)
        {
            string sortString;
            switch (sortType)
            {
                case "name":
                    sortString = "@Name";
                    break;
                case "description":
                    sortString = "@Description";
                    break;
                default:
                    sortType = null;
                    sortString = "@PublicationDate DESC";
                    break;
            }
            return sortString;
        }
        public static string GetAdminSortSqlString(ref string sortType)
        {
            string sortString;
            switch (sortType)
            {
                case "id":
                    sortString = "@Id";
                    break;
                case "name":
                    sortString = "@Name";
                    break;
                case "description":
                    sortString = "@Description";
                    break;
                case "editDate":
                    sortString = "@EditDate DESC";
                    break;
                case "Author":
                    sortString = "@AuthorId";
                    break;
                case "publishDate":
                    sortString = "@PublicationDate DESC";
                    break;
                case "visibility":
                    sortString = "@IsVisible";
                    break;
                default:
                    sortType = null;
                    sortString = "@CreatedDate DESC";
                    break;
            }
            return sortString;
        }
        public static string GetSearchSqlString(ref string searchType)
        {
            string searchString;
            switch (searchType)
            {
                case "description":
                    searchString = "Description LIKE :searchName ";
                    break;
                default:
                    searchType = null;
                    searchString = "Name LIKE :searchName ";
                    break;
            }
            return searchString;
        }
        public static NewsPostViewModel GetNewsPostViewModelById(int id, int page)
        {
            using (ISession session = NHibernateSession.OpenSession())
            {
                NewsPost newsPost = session.Get<NewsPost>(id);
                List<CommentViewModel> commentsViewModel = new List<CommentViewModel>();

                IEnumerable<Comment> comments = session.CreateQuery("from Comment where NewsPostId = :id ORDER BY @Date DESC").SetParameter("id", id).List<Comment>();

                int commentsCount = comments.Count();
                int pages = commentsCount / Comment.CommentsInOnePage;
                if (commentsCount % Comment.CommentsInOnePage != 0)
                    pages++;
                comments = comments.Skip(Comment.CommentsInOnePage * page).Take(Comment.CommentsInOnePage);
                foreach (Comment comment in comments)
                {
                    commentsViewModel.Add(new CommentViewModel(comment));
                }
                return new NewsPostViewModel(newsPost, commentsViewModel, page, pages);
            }
        }
        public static IQuery GetSqlQuerry(ISession session, string sortType, string filter, string search, string searchString)
        {
            search = search ?? "_";
            return session.CreateQuery("from NewsPost where " +
                   $"{filter} AND " +
                    searchString +
                    "AND IsVisible = 1 AND PublicationDate <= GetDate() " +
                   $"ORDER BY {sortType}").
                    SetParameter("searchName", $"%{search}%");
        }
        public static IQuery GetSqlQuerryAdmin(ISession session, string sortType, string filter, string search, string searchString)
        {
            search = search ?? "_";
            return session.CreateQuery("from NewsPost where " +
                   $"{filter} AND " +
                    searchString +
                   $"ORDER BY {sortType}").
                    SetParameter("searchName", $"%{search}%");
        }
    }
}
