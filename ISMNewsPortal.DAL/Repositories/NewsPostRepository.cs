using ISMNewsPortal.DAL.Interfaces;
using ISMNewsPortal.DAL.Models;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ISMNewsPortal.DAL.ToolsLogic.NewsPostToolsLogic;

namespace ISMNewsPortal.DAL.Repositories
{
    public class NewsPostRepository : IRepository<NewsPost>
    {
        private ISession session;
        public NewsPostRepository(ISession session)
        {
            this.session = session;
        }

        public int Create(NewsPost item)
        {
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Save(item);
                transaction.Commit();
                return item.Id;
            }
        }

        public void Delete(int id)
        {
            var item = session.Get<NewsPost>(id);
            if (item == null)
                return;
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Delete(item);
                transaction.Commit();
            }
        }

        public IEnumerable<NewsPost> Find(Func<NewsPost, bool> predicate)
        {
            return session.Query<NewsPost>().Where(predicate);
        }

        public NewsPost Get(int id)
        {
            return session.Get<NewsPost>(id);
        }

        public IEnumerable<NewsPost> GetAll()
        {
            return session.Query<NewsPost>();
        }

        public int Count()
        {
            return session.Query<NewsPost>().Count();
        }

        public int Count(Func<NewsPost, bool> predicate)
        {
            return session.Query<NewsPost>().Count(predicate);
        }

        public void Update(NewsPost item)
        {
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Update(item);
                transaction.Commit();
            }
        }

        public bool Any(Func<NewsPost, bool> predicate)
        {
            return session.Query<NewsPost>().Any(predicate);
        }

        public IEnumerable<NewsPost> GetAllWithTools(ToolBarModel model)
        {
            if (model.Admin)
                return GetAllWithAdminTools(model);
            else
                return GetAllWithoutAdminTools(model);
        }

        private IEnumerable<NewsPost> GetAllWithoutAdminTools(ToolBarModel model)
        {
            string filterFunc = GetFilterSqlString(model.Filter);
            string sortString = GetAdminSortSqlString(model.SortType, model.Reversed ?? false);
            string searchString = GetSearchSqlString();
            IList<NewsPost> selectedNewsPost = GetSqlQuerry(session, sortString, filterFunc, model.Search, searchString).List<NewsPost>();

            model.Pages = Helper.CalculatePages(selectedNewsPost.Count, NewsPost.NewsInOnePage);

            return Helper.CutIEnumarable(model.Page, NewsPost.NewsInOnePage, selectedNewsPost);
        }

        private IEnumerable<NewsPost> GetAllWithAdminTools(ToolBarModel model)
        {
            string filterFunc = GetFilterSqlString(model.Filter);
            string sortString = GetAdminSortSqlString(model.SortType, model.Reversed ?? false);
            string searchString = GetSearchSqlString();
            IList<NewsPost> selectedNewsPost = GetSqlQuerryAdmin(session, sortString, filterFunc, model.Search, searchString).List<NewsPost>();

            model.Pages = Helper.CalculatePages(selectedNewsPost.Count, NewsPost.NewsInOnePage);

            return Helper.CutIEnumarable(model.Page, NewsPost.NewsInOnePage, selectedNewsPost);
        }

        public NewsPost FindSingle(Func<NewsPost, bool> predicate)
        {
            return session.Query<NewsPost>().SingleOrDefault(predicate);
        }

        public U Max<U>(Func<NewsPost, U> predicate)
        {
            return session.Query<NewsPost>().Max(predicate);
        }
    }
}
