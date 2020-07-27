using ISMNewsPortal.BLL.Repositories;
using ISMNewsPortal.DAL.Lucene;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ISMNewsPortal.DAL.ToolsLogic.NewsPostToolsLogic;
using ISMNewsPortal.BLL.Models;
using ISMNewsPortal.BLL.BusinessModels;

namespace ISMNewsPortal.DAL.Repositories
{
    public class NewsPostRepository : Repository<NewsPost>, INewsPostRepository
    {
        public NewsPostRepository(ISession session) : base(session)
        {
        }

        public int GetCommentsCount(int postId)
        {
            return _session.Query<Comment>().Count(u => u.NewsPostId == postId);
        }

        public override IEnumerable<NewsPost> GetWithOptions(Options toolBar)
        {
            string filterFunc = GetFilterSqlString(toolBar.Filter);
            string sortString;
            string searchString = GetSearchSqlString();
            IList<NewsPost> selectedNewsPost;
            if (toolBar.Admin)
            {
                sortString = GetAdminSortSqlString(toolBar.SortType, toolBar.Reversed ?? true);
                selectedNewsPost = GetSqlQuerryAdmin(_session, sortString, filterFunc, toolBar.Search, searchString).List<NewsPost>();
            }
            else
            {
                sortString = GetSortSqlString(toolBar.SortType, toolBar.Reversed ?? true);
                selectedNewsPost = GetSqlQuerry(_session, sortString, filterFunc, toolBar.Search, searchString).List<NewsPost>();
            }
            toolBar.Pages = Helper.CalculatePages(selectedNewsPost.Count, NewsPost.NewsInOnePage);

            return Helper.CutIEnumarable(toolBar.Page - 1, NewsPost.NewsInOnePage, selectedNewsPost);
        }

        public IEnumerable<NewsPost> GetByAuthorId(int id)
        {
            return _session.Query<NewsPost>().Where(u => u.AuthorId == id);
        }

        public IEnumerable<NewsPost> GetByImageId(int id)
        {
            return _session.Query<NewsPost>().Where(u => u.ImageId == id);
        }

        public IEnumerable<NewsPost> GetByName(string name)
        {
            return _session.Query<NewsPost>().Where(u => u.Name == name);
        }

        public IEnumerable<NewsPost> GetByVisibility(bool visible)
        {
            return _session.Query<NewsPost>().Where(u => u.IsVisible == visible);
        }

        public override void Update(NewsPost item)
        {
            var createdNewsPost = _session.Get<NewsPost>(item.Id);
            DateTime createdDate = createdNewsPost.CreatedDate;
            item.CreatedDate = createdDate;
            using (ITransaction transaction = _session.BeginTransaction())
            {
                _session.Update(item);
                transaction.Commit();
            }
        }
    }
}
