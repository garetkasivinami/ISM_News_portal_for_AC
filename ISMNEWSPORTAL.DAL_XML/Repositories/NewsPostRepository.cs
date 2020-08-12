using ISMNewsPortal.BLL.BusinessModels;
using ISMNewsPortal.BLL.Models;
using ISMNewsPortal.BLL.Repositories;
using ISMNewsPortal.Lucene;
using System;
using System.Collections.Generic;
using System.Linq;
using static ISMNewsPortal.BLL.Services.NewsPostService;

namespace ISMNewsPortal.DAL_XML.Repositories
{
    public class NewsPostRepository : Repository<NewsPost>, INewsPostRepository
    {
        public NewsPostRepository(XMLContex contex) : base(contex) 
        {
            var items = GetAll();
            var luceneRepository = LuceneRepositoryFactory.GetRepository<NewsPost>();
            luceneRepository.DeleteAll();
            luceneRepository.SaveOrUpdate(items);
        }

        public IEnumerable<NewsPost> GetByAuthorId(int id)
        {
            return GetAll().Where(u => u.AuthorId == id);
        }

        public IEnumerable<NewsPost> GetByImageId(int id)
        {
            return GetAll().Where(u => u.ImageId == id);
        }

        public IEnumerable<NewsPost> GetByName(string name)
        {
            return GetAll().Where(u => u.Name == name);
        }

        public IEnumerable<NewsPost> GetByVisibility(bool visible)
        {
            return GetAll().Where(u => u.IsVisible == visible);
        }

        public int GetCommentsCount(int postId)
        {
            return contex.GetAll<Comment>().Where(u => u.NewsPostId == postId).Count();
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

        public static int CalculatePages(int count, int countInOnePage)
        {
            int pages = count / countInOnePage;
            if (count % countInOnePage != 0)
            {
                pages++;
            }
            return pages;
        }

        public override void Update(NewsPost item)
        {
            LuceneRepositoryFactory.GetRepository<NewsPost>().SaveOrUpdate(item);
            base.Update(item);
        }

        public override IEnumerable<NewsPost> GetWithOptions(object requirements)
        {
            Options options = requirements as Options;
            IEnumerable<NewsPost> result;
            if (!string.IsNullOrEmpty(options.Search))
                result = LuceneRepositoryFactory.GetRepository<NewsPost>().Search(options);
            else
                result = base.GetWithOptions(options);

            if (!options.Admin)
            {
                result = result.Where(u => u.IsVisible == true && u.PublicationDate < DateTime.Now);
            } else {
                var ids = result.Select(u => u.Id);
                result = GetAll().Where(u => ids.Contains(u.Id));
            }

            if (options.Reversed == true || options.Reversed == null)
                result = SortByReversed(result, options.SortType);
            else
                result = SortBy(result, options.SortType);

            if (options.MinimumDate != null)
                result = result.Where(u => u.PublicationDate >= options.MinimumDate);

            if (options.MaximumDate != null)
                result = result.Where(u => u.PublicationDate < options.MaximumDate);

            if (options.Published != null)
            {
                if (options.Published == true)
                    result = result.Where(u => u.IsVisible == true);
                else
                    result = result.Where(u => u.IsVisible == false);
            }

            options.Pages = CalculatePages(result.Count(), NewsPost.NewsInOnePage);

            result = result.Skip(options.Page * NewsPost.NewsInOnePage).Take(NewsPost.NewsInOnePage);

            return result;
        }
    }
}
