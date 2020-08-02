using ISMNewsPortal.BLL.BusinessModels;
using ISMNewsPortal.BLL.Models;
using ISMNewsPortal.BLL.Repositories;
using ISMNewsPortal.Lucene;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISMNEWSPORTAL.DAL_XML.Repositories
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

        public override void Update(NewsPost item)
        {
            LuceneRepositoryFactory.GetRepository<NewsPost>().SaveOrUpdate(item);
            base.Update(item);
        }

        public override IEnumerable<NewsPost> GetWithOptions(Options toolBar)
        {

            IEnumerable<NewsPost> result;
            if (!string.IsNullOrEmpty(toolBar.Search))
            {
                var results = LuceneRepositoryFactory.GetRepository<NewsPost>().Search(toolBar);
                result = GetAll().Where(u => results.Contains(u.Id));
            } else
            {
                result = base.GetWithOptions(toolBar);
            }
            if (toolBar.MinimumDate != null)
                result = result.Where(u => u.PublicationDate >= toolBar.MinimumDate);

            if (toolBar.MaximumDate != null)
                result = result.Where(u => u.PublicationDate < toolBar.MaximumDate);

            if (toolBar.Published != null)
            {
                if (toolBar.Published == true)
                    result = result.Where(u => u.IsVisible == true);
                else
                    result = result.Where(u => u.IsVisible == false);
            }

            if (!toolBar.Admin)
                result = result.Where(u => u.IsVisible == true && u.PublicationDate < DateTime.Now);

            return result;
        }
    }
}
