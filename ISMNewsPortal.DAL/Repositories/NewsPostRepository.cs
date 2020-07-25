using ISMNewsPortal.BLL.Interfaces;
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

        public int CommentsCount(int postId)
        {
            return session.Query<Comment>().Count(u => u.NewsPostId == postId);
        }

        public int Count()
        {
            return session.Query<NewsPost>().Count();
        }

        public int Create(NewsPost item)
        {
            var newsPost = MapFromNewsPostDTO<NewsPost>(item);
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Save(newsPost);
                transaction.Commit();
                return newsPost.Id;
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
            var newsPost = session.Get<NewsPost>(id);
            return MapToNewsPostDTO(newsPost);
        }

        public IEnumerable<NewsPost> GetAll()
        {
            var newsPosts = session.Query<NewsPost>();
            return MapToNewsPostDTOList(newsPosts);
        }

        public IEnumerable<NewsPost> GetAllWithTools(ToolsDTO toolBar)
        {
            string filterFunc = GetFilterSqlString(toolBar.Filter);
            string sortString = GetAdminSortSqlString(toolBar.SortType, toolBar.Reversed ?? false);
            string searchString = GetSearchSqlString();
            IList<NewsPost> selectedNewsPost;
            if (toolBar.Admin)
                selectedNewsPost = GetSqlQuerryAdmin(session, sortString, filterFunc, toolBar.Search, searchString).List<NewsPost>();
            else
                selectedNewsPost = GetSqlQuerry(session, sortString, filterFunc, toolBar.Search, searchString).List<NewsPost>();

            toolBar.Pages = Helper.CalculatePages(selectedNewsPost.Count, NewsPost.NewsInOnePage);

            IEnumerable<NewsPost> result = Helper.CutIEnumarable(toolBar.Page, NewsPost.NewsInOnePage, selectedNewsPost);

            return MapToNewsPostDTOList(result);
        }

        public IEnumerable<NewsPost> GetByAuthorId(int id)
        {
            var newsPosts = session.Query<NewsPost>().Where(u => u.AuthorId == id);
            return MapToNewsPostDTOList(newsPosts);
        }

        public IEnumerable<NewsPost> GetByImageId(int id)
        {
            var newsPosts = session.Query<NewsPost>().Where(u => u.ImageId == id);
            return MapToNewsPostDTOList(newsPosts);
        }

        public IEnumerable<NewsPost> GetByName(string name)
        {
            var newsPosts = session.Query<NewsPost>().Where(u => u.Name == name);
            return MapToNewsPostDTOList(newsPosts);
        }

        public IEnumerable<NewsPost> GetByVisibility(bool visible)
        {
            var newsPosts = session.Query<NewsPost>().Where(u => u.IsVisible == visible);
            return MapToNewsPostDTOList(newsPosts);
        }

        public void Update(NewsPost item)
        {
            var newsPost = MapFromNewsPostDTO<NewsPost>(item);
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
