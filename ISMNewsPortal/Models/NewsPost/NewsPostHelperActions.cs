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
        public static string GetFilterSqlString(string filter)
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
                    filterFunc = NewsPostHelperActions.FilterAll();
                    break;
            }
            return filterFunc;
        }
        public static string GetSortSqlString(string sortType)
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
                    sortString = "@PublicationDate DESC";
                    break;
            }
            return sortString;
        }
        public static string GetAdminSortSqlString(string sortType, bool reversed)
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
                    sortString = "@EditDate";
                    break;
                case "Author":
                    sortString = "@AuthorId";
                    break;
                case "publishDate":
                    reversed = !reversed;
                    sortString = "@PublicationDate";
                    break;
                case "visibility":
                    sortString = "@IsVisible";
                    break;
                default:
                    reversed = !reversed;
                    sortString = "@CreatedDate";
                    break;
            }
            if (reversed)
                sortString += " DESC";
            return sortString;
        }
        public static string GetSearchSqlString(string searchType)
        {
            string searchString;
            switch (searchType)
            {
                case "description":
                    searchString = "Description LIKE :searchName ";
                    break;
                default:
                    searchString = "Name LIKE :searchName ";
                    break;
            }
            return searchString;
        }
        public static NewsPostViewModel GetNewsPostViewModelById(int id, int page, bool checkVisibility = false)
        {
            using (ISession session = NHibernateSession.OpenSession())
            {
                NewsPost newsPost = session.Get<NewsPost>(id);
                if (checkVisibility && (!newsPost.IsVisible || newsPost.PublicationDate > DateTime.Now))
                {
                    return null;
                }
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
            return session.CreateQuery($"from NewsPost where {filter} AND {searchString} AND IsVisible = 1 AND PublicationDate <= GetDate() ORDER BY {sortType}").
                    SetParameter("searchName", $"%{search}%");
        }
        public static IQuery GetSqlQuerryAdmin(ISession session, string sortType, string filter, string search, string searchString)
        {
            search = search ?? "_";
            return session.CreateQuery($"from NewsPost where {filter} AND {searchString} ORDER BY {sortType}").
                    SetParameter("searchName", $"%{search}%");
        }
        public static NewsPostAdminCollection GenerateNewsPostAdminCollection(ToolBarModel model)
        {
            using (ISession session = NHibernateSession.OpenSession())
            {
                string filterFunc = GetFilterSqlString(model.Filter);
                string sortString = GetAdminSortSqlString(model.SortType, model.Reversed ?? false);
                string searchString = GetSearchSqlString(model.TypeSearch);
                IEnumerable<NewsPost> selectedNewsPost = GetSqlQuerryAdmin(session, sortString, filterFunc, model.Search, searchString).List<NewsPost>();

                int newsCount = selectedNewsPost.Count();

                selectedNewsPost = CutIEnumarable(model.Page, NewsPost.NewsInOnePage, selectedNewsPost);
                ICollection<NewsPostAdminView> newsPostsAdminView = new List<NewsPostAdminView>();
                foreach (NewsPost newsPost in selectedNewsPost)
                {
                    string newsPostAuthorName = session.Get<Admin>(newsPost.AuthorId).Login;
                    int commentCount = session.Query<Comment>().Where(u => u.NewsPostId == newsPost.Id).Count();
                    newsPostsAdminView.Add(new NewsPostAdminView(newsPost, newsPostAuthorName, commentCount));
                }
                model.Pages = CalculatePages(newsCount, NewsPost.NewsInOnePage);
                return new NewsPostAdminCollection(newsPostsAdminView, model);
            }
        }
        public static NewsPostSimplifiedCollection GenerateNewsPostSimplifiedCollection(ToolBarModel model)
        {
            using (ISession session = NHibernateSession.OpenSession())
            {
                string filterFunc = GetFilterSqlString(model.Filter);
                string sortString = GetAdminSortSqlString(model.SortType, model.Reversed ?? false);
                string searchString = GetSearchSqlString(model.TypeSearch);
                IEnumerable<NewsPost> selectedNewsPost = GetSqlQuerry(session, sortString, filterFunc, model.Search, searchString).List<NewsPost>();

                int newsCount = selectedNewsPost.Count();

                selectedNewsPost = CutIEnumarable(model.Page, NewsPost.NewsInOnePage, selectedNewsPost);
                ICollection<NewsPostSimplifiedView> newsPostSimplifiedViews = new List<NewsPostSimplifiedView>();
                foreach (NewsPost newsPost in selectedNewsPost)
                {
                    int commentCount = session.Query<Comment>().Where(u => u.NewsPostId == newsPost.Id).Count();
                    newsPostSimplifiedViews.Add(new NewsPostSimplifiedView(newsPost, commentCount));
                }
                model.Pages = CalculatePages(newsCount, NewsPost.NewsInOnePage);
                return new NewsPostSimplifiedCollection(newsPostSimplifiedViews, model);
            }
        }
        public static NewsPostAdminView GetNewsPostAdminView(int id)
        {
            using (ISession session = NHibernateSession.OpenSession())
            {
                NewsPost newsPost = session.Get<NewsPost>(id);
                string newsPostAuthorName = session.Get<Admin>(newsPost.AuthorId).Login;
                int commentCount = session.Query<Comment>().Where(u => u.NewsPostId == newsPost.Id).Count();
                NewsPostAdminView newsPostAdminView = new NewsPostAdminView(newsPost, newsPostAuthorName, commentCount);
                return newsPostAdminView;
            }
        }
        public static NewsPostEditModel GetNewsPostEditModel(int id)
        {
            using (ISession session = NHibernateSession.OpenSession())
            {
                NewsPost newsPost = session.Get<NewsPost>(id);
                NewsPostEditModel newsPostAdminView = new NewsPostEditModel(newsPost);
                return newsPostAdminView;
            }
        }
    }
}
