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
    }
}
