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
using ISMNewsPortal.Lucene;

namespace ISMNewsPortal.DAL.Repositories
{
    public class NewsPostRepository : Repository<NewsPost>, INewsPostRepository
    {
        public NewsPostRepository(ISession session) : base(session)
        {
            var items = GetAll();
            var luceneRepository = LuceneRepositoryFactory.GetRepository<NewsPost>();
            luceneRepository.DeleteAll();
            luceneRepository.SaveOrUpdate(items);
        }

        public int GetCommentsCount(int postId)
        {
            return _session.Query<Comment>().Count(u => u.NewsPostId == postId);
        }

        public override IEnumerable<NewsPost> GetWithOptions(object requirements)
        {
            var options = requirements as Options;
            IEnumerable<NewsPost> items;

            if (!string.IsNullOrEmpty(options.Search))
                items = LuceneRepositoryFactory.GetRepository<NewsPost>().Search(options);
            else
                items = _session.Query<NewsPost>();

            if (!options.Admin)
            {
                items = items.Where(u => u.IsVisible == true && u.PublicationDate < DateTime.Now);
            } else {
                var ids = items.Select(u => u.Id);
                items = _session.Query<NewsPost>().Where(u => ids.Contains(u.Id));
            }

            if (options.Reversed == true || options.Reversed == null)
                items = SortByReversed(items, options.SortType);
            else
                items = SortBy(items, options.SortType);

            if (options.MinimumDate != null)
                items = items.Where(u => u.PublicationDate >= options.MinimumDate);

            if (options.MaximumDate != null)
                items = items.Where(u => u.PublicationDate < options.MaximumDate);

            if (options.Published != null)
            {
                if (options.Published == true)
                    items = items.Where(u => u.IsVisible == true);
                else
                    items = items.Where(u => u.IsVisible == false);
            }
                

            options.Pages = Helper.CalculatePages(items.Count(), NewsPost.NewsInOnePage);

            items = items.Skip(options.Page * NewsPost.NewsInOnePage).Take(NewsPost.NewsInOnePage);

            var result = items.ToList();

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
            LuceneRepositoryFactory.GetRepository<NewsPost>().SaveOrUpdate(item);
            _session.Update(item);
        }
        public override int Create(NewsPost item)
        {
            LuceneRepositoryFactory.GetRepository<NewsPost>().SaveOrUpdate(item);
            return base.Create(item);
        }

        public override void Delete(int id)
        {
            LuceneRepositoryFactory.GetRepository<NewsPost>().Delete(id);
            base.Delete(id);
        }
    }
}
