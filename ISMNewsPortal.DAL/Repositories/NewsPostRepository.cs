using ISMNewsPortal.BLL.Repositories;
using ISMNewsPortal.DAL.Models;
using ISMNewsPortal.DAL.Lucene;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ISMNewsPortal.BLL.Mappers.Automapper;
using static ISMNewsPortal.DAL.ToolsLogic.NewsPostToolsLogic;
using ISMNewsPortal.BLL.Models;
using ISMNewsPortal.BLL.BusinessModels;

namespace ISMNewsPortal.DAL.Repositories
{
    public class NewsPostRepository : INewsPostRepository
    {
        private ISession session;
        public NewsPostRepository(ISession session)
        {
            this.session = session;
        }

        public int GetCommentsCount(int postId)
        {
            return session.Query<Comment>().Count(u => u.NewsPostId == postId);
        }

        public int Count()
        {
            return session.Query<NewsPost>().Count();
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
            var newsPost = session.Get<NewsPost>(id);
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Save(newsPost);
                transaction.Commit();
            }
        }

        public NewsPost Get(int id)
        {
            return session.Get<NewsPost>(id);
        }

        public IEnumerable<NewsPost> GetAll()
        {
            return session.Query<NewsPost>();
        }

        public IEnumerable<NewsPost> GetWithOptions(ToolsDTO toolBar)
        {
            string filterFunc = GetFilterSqlString(toolBar.Filter);
            string sortString;
            string searchString = GetSearchSqlString();
            IList<NewsPost> selectedNewsPost;
            if (toolBar.Admin)
            {
                sortString = GetAdminSortSqlString(toolBar.SortType, toolBar.Reversed ?? true);
                selectedNewsPost = GetSqlQuerryAdmin(session, sortString, filterFunc, toolBar.Search, searchString).List<NewsPost>();
            }
            else
            {
                sortString = GetSortSqlString(toolBar.SortType, toolBar.Reversed ?? true);
                selectedNewsPost = GetSqlQuerry(session, sortString, filterFunc, toolBar.Search, searchString).List<NewsPost>();
            }
            toolBar.Pages = Helper.CalculatePages(selectedNewsPost.Count, NewsPost.NewsInOnePage);

            return Helper.CutIEnumarable(toolBar.Page, NewsPost.NewsInOnePage, selectedNewsPost);
        }

        public IEnumerable<NewsPost> GetByAuthorId(int id)
        {
            return session.Query<NewsPost>().Where(u => u.AuthorId == id);
        }

        public IEnumerable<NewsPost> GetByImageId(int id)
        {
            return session.Query<NewsPost>().Where(u => u.ImageId == id);
        }

        public IEnumerable<NewsPost> GetByName(string name)
        {
            return session.Query<NewsPost>().Where(u => u.Name == name);
        }

        public IEnumerable<NewsPost> GetByVisibility(bool visible)
        {
            return session.Query<NewsPost>().Where(u => u.IsVisible == visible);
        }

        public void Update(NewsPost item)
        {
            var newsPost = item;
            var createdNewsPost = session.Get<NewsPost>(item.Id);
            DateTime createdDate = createdNewsPost.CreatedDate;
            Map(newsPost, createdNewsPost);
            createdNewsPost.CreatedDate = createdDate;
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Update(createdNewsPost);
                transaction.Commit();
            }
        }
    }
}
