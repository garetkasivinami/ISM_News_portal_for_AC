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
    using System.Web;

    public partial class NewsPost
    {
        public const int NewsInOnePage = 10;
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
        public virtual DateTime CreatedDate { get; set; }
        public virtual DateTime? EditDate { get; set; }
        public virtual int AuthorId { get; set; }
        public virtual bool IsVisible { get; set; }
        public virtual DateTime PublicationDate { get; set; }
        public virtual int ImageId { get; set; }
        //===============================================================================
        public NewsPost()
        {

        }
        public NewsPost(NewsPostCreateModel model)
        {
            Name = model.Name;
            Description = model.Description;
            CreatedDate = DateTime.Now;
            EditDate = null;
            ImageId = model.ImageId;
            AuthorId = model.AuthorId;
            IsVisible = model.IsVisible;
            PublicationDate = model.PublicationDate ?? DateTime.Now;
        }
        public NewsPost(NewsPostEditModel model)
        {
            Id = model.Id;
            Name = model.Name;
            Description = model.Description;
            CreatedDate = DateTime.Now;
            EditDate = null;
            ImageId = model.ImageId;
            AuthorId = model.AuthorId;
            IsVisible = model.IsVisible;
            PublicationDate = model.PublicationDate ?? DateTime.Now;
        }
        public static void Update(NewsPostEditModel model)
        {
            using (ISession session = NHibernateSession.OpenSession())
            {
                NewsPost newsPost = new NewsPost(model);
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Update(newsPost);
                    transaction.Commit();
                }
            }
        }
        public static NewsPostAdminCollection GenerateNewsPostAdminCollection(ToolBarModel model)
        {
            using (ISession session = NHibernateSession.OpenSession())
            {
                string filterFunc = NewsPostHelperActions.GetFilterSqlString(model.Filter);
                string sortString = NewsPostHelperActions.GetAdminSortSqlString(model.SortType);
                string searchString = NewsPostHelperActions.GetSearchSqlString(model.TypeSearch);
                IEnumerable<NewsPost> selectedNewsPost = NewsPostHelperActions.GetSqlQuerryAdmin(session, sortString, filterFunc, model.Search, searchString).List<NewsPost>();

                int newsCount = selectedNewsPost.Count();

                selectedNewsPost = NewsPostHelperActions.CutIEnumarable(model.Page, NewsInOnePage, selectedNewsPost);
                ICollection<NewsPostAdminView> newsPostsAdminView = new List<NewsPostAdminView>();
                foreach (NewsPost newsPost in selectedNewsPost)
                {
                    string newsPostAuthorName = session.Get<Admin>(newsPost.AuthorId).Login;
                    int commentCount = session.Query<Comment>().Where(u => u.NewsPostId == newsPost.Id).Count();
                    newsPostsAdminView.Add(new NewsPostAdminView(newsPost, newsPostAuthorName, commentCount));
                }
                model.Pages = NewsPostHelperActions.CalculatePages(newsCount, NewsInOnePage);
                return new NewsPostAdminCollection(newsPostsAdminView, model);
            }
        }
        public static NewsPostSimplifiedCollection GenerateNewsPostSimplifiedCollection(ToolBarModel model)
        {
            using (ISession session = NHibernateSession.OpenSession())
            {
                string filterFunc = NewsPostHelperActions.GetFilterSqlString(model.Filter);
                string sortString = NewsPostHelperActions.GetAdminSortSqlString(model.SortType);
                string searchString = NewsPostHelperActions.GetSearchSqlString(model.TypeSearch);
                IEnumerable<NewsPost> selectedNewsPost = NewsPostHelperActions.GetSqlQuerryAdmin(session, sortString, filterFunc, model.Search, searchString).List<NewsPost>();

                int newsCount = selectedNewsPost.Count();

                selectedNewsPost = NewsPostHelperActions.CutIEnumarable(model.Page, NewsInOnePage, selectedNewsPost);
                ICollection<NewsPostSimplifiedView> newsPostSimplifiedViews = new List<NewsPostSimplifiedView>();
                foreach (NewsPost newsPost in selectedNewsPost)
                {
                    int commentCount = session.Query<Comment>().Where(u => u.NewsPostId == newsPost.Id).Count();
                    newsPostSimplifiedViews.Add(new NewsPostSimplifiedView(newsPost, commentCount));
                }
                model.Pages = NewsPostHelperActions.CalculatePages(newsCount, NewsInOnePage);
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
                FileModel.Delete(newsPost.ImageId);
            }
        }
        public static void AddNewsPost(NewsPostCreateModel model)
        {
            using (ISession session = NHibernateSession.OpenSession())
            {
                NewsPost newsPost = new NewsPost(model);
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Save(newsPost);
                    transaction.Commit();
                }
            }
        }
    }
}
