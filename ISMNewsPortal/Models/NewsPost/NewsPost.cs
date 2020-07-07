namespace ISMNewsPortal.Models
{
    using NHibernate;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Data.SqlTypes;
    using System.Linq;

    public partial class NewsPost
    {
        public const int NewsInOnePage = 10;
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
        public virtual DateTime CreatedDate { get; set; }
        public virtual DateTime? EditDate { get; set; }
        public virtual string ImagePath { get; set; }
        public virtual int AuthorId { get; set; }
        public virtual bool IsVisible { get; set; }
        public virtual DateTime PublicationDate { get; set; }
        public static NewsPostAdminCollection GenerateNewsPostAdminCollection(int page, string sortType, string filter, string search, string typeSearch)
        {
            using (ISession session = NHibernateSession.OpenSession())
            {
                string filterFunc = NewsPostHelperActions.GetFilterSqlString(ref filter);
                string sortString = NewsPostHelperActions.GetAdminSortSqlString(ref sortType);
                string searchString = NewsPostHelperActions.GetSearchSqlString(ref typeSearch);
                IEnumerable<NewsPost> selectedNewsPost = NewsPostHelperActions.GetSqlQuerryAdmin(session, sortString, filterFunc, search, searchString).List<NewsPost>();

                int newsCount = selectedNewsPost.Count();

                selectedNewsPost = NewsPostHelperActions.CutIEnumarable(page, NewsInOnePage, selectedNewsPost);
                ICollection<NewsPostAdminView> newsPostsAdminView = new List<NewsPostAdminView>();
                foreach (NewsPost newsPost in selectedNewsPost)
                {
                    string newsPostAuthorName = session.Get<Admin>(newsPost.AuthorId).Login;
                    int commentCount = session.Query<Comment>().Where(u => u.NewsPostId == newsPost.Id).Count();
                    newsPostsAdminView.Add(new NewsPostAdminView(newsPost, newsPostAuthorName, commentCount));
                }
                return new NewsPostAdminCollection()
                {
                    Filter = filter,
                    SortType = sortType,
                    Page = page,
                    Pages = NewsPostHelperActions.CalculatePages(newsCount, NewsInOnePage),
                    NewsPostAdminViews = newsPostsAdminView,
                    Search = search
                };
            }
        }
        public static NewsPostSimplifiedCollection GenerateNewsPostSimplifiedCollection(int page, string sortType, string filter, string search, string typeSearch)
        {
            using (ISession session = NHibernateSession.OpenSession())
            {
                string filterFunc = NewsPostHelperActions.GetFilterSqlString(ref filter);
                string sortString = NewsPostHelperActions.GetAdminSortSqlString(ref sortType);
                string searchString = NewsPostHelperActions.GetSearchSqlString(ref typeSearch);
                IEnumerable<NewsPost> selectedNewsPost = NewsPostHelperActions.GetSqlQuerryAdmin(session, sortString, filterFunc, search, searchString).List<NewsPost>();

                int newsCount = selectedNewsPost.Count();

                selectedNewsPost = NewsPostHelperActions.CutIEnumarable(page, NewsInOnePage, selectedNewsPost);
                ICollection<NewsPostSimplifiedView> newsPostSimplifyViews = new List<NewsPostSimplifiedView>();
                foreach (NewsPost newsPost in selectedNewsPost)
                {
                    int commentCount = session.Query<Comment>().Where(u => u.NewsPostId == newsPost.Id).Count();
                    newsPostSimplifyViews.Add(new NewsPostSimplifiedView(newsPost, commentCount));
                }
                return new NewsPostSimplifiedCollection()
                {
                    NewsPostSimpliedViews = newsPostSimplifyViews,
                    Pages = NewsPostHelperActions.CalculatePages(newsCount, NewsInOnePage),
                    Page = page,
                    Filter = filter,
                    SortType = sortType,
                    Search = search
                };
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
        public static void RemoveNewsPost(int id)
        {
            using (ISession session = NHibernateSession.OpenSession())
            {
                NewsPost newsPost = session.Get<NewsPost>(id);
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Delete(newsPost);
                    transaction.Commit();
                }
            }
        }
    }
}
