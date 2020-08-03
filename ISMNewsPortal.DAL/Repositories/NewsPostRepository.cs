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
using ISMNewsPortal.DAL.ToolsLogic;

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
            var items = _session.Query<NewsPost>();

            if(toolBar.Reversed == true || toolBar.Reversed == null)
                items = SortByReversed(items, toolBar.SortType);
            else
                items = SortBy(items, toolBar.SortType);

            if (toolBar.MinimumDate != null)
                items = items.Where(u => u.PublicationDate >= toolBar.MinimumDate);

            if (toolBar.MaximumDate != null)
                items = items.Where(u => u.PublicationDate < toolBar.MaximumDate);

            if (toolBar.Published != null)
            {
                if (toolBar.Published == true)
                    items = items.Where(u => u.IsVisible == true);
                else
                    items = items.Where(u => u.IsVisible == false);
            }

            if (!toolBar.Admin)
                items = items.Where(u => u.IsVisible == true && u.PublicationDate < DateTime.Now);

            items = items.Skip((toolBar.Page - 1) * NewsPost.NewsInOnePage).Take(NewsPost.NewsInOnePage);

            var result = items.ToList();

            toolBar.Pages = Helper.CalculatePages(result.Count, NewsPost.NewsInOnePage);

            return result;
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
