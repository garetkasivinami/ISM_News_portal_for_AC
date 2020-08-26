using System.Collections.Generic;
using System.Linq;
using ISMNewsPortal.BLL.Models;
using ISMNewsPortal.BLL.BusinessModels;
using ISMNewsPortal.BLL.Exceptions;
using static ISMNewsPortal.BLL.SessionManager;
using System.Net;
using System.Web.Caching;
using System.Runtime.Caching;
using ISMNewsPortal.BLL.Repositories;

namespace ISMNewsPortal.BLL.Services
{
    public class NewsPostService
    {
        public IEnumerable<NewsPost> GetNewsPosts()
        {
            return NewsPostRepository.GetAll();
        }

        public IEnumerable<NewsPost> GetNewsPostsWithTools(Options options)
        {
            IEnumerable<NewsPost> newsPosts;
            if (string.IsNullOrEmpty(options.Search))
                newsPosts = NewsPostRepository.GetWithOptions(options);
            else
                newsPosts = LuceneRepositoryFactory.GetRepository<NewsPost>().Search(options);

            return newsPosts;
        }

        public IEnumerable<NewsPost> GetNewsPostsWithAdminTools(Options options)
        {
            options.Admin = true;
            return GetNewsPostsWithTools(options);
        }

        public NewsPost GetNewsPost(int id)
        {
            var newsPost = CacheRepository.GetItem(id, typeof(NewsPost));
            if (newsPost != null)
                return newsPost as NewsPost;

            newsPost = NewsPostRepository.Get(id);
            if (newsPost == null)
                throw new NewsPostNullException();
            CacheRepository.AddItem(newsPost);
            return newsPost as NewsPost;
        }

        public void UpdateNewsPost(NewsPost newsPost)
        {
            NewsPostRepository.Update(newsPost);
            CacheRepository.Update(newsPost);
            LuceneRepositoryFactory.GetRepository<NewsPost>().SaveOrUpdate(newsPost);
            UnitOfWork.Save();
        }

        public void CreateNewsPost(NewsPost newsPost)
        {
            NewsPostRepository.Create(newsPost);
            CacheRepository.AddItem(newsPost);
            LuceneRepositoryFactory.GetRepository<NewsPost>().SaveOrUpdate(newsPost);
            UnitOfWork.Save();
        }

        public void DeleteNewsPost(int id)
        {
            NewsPostRepository.Delete(id);
            CommentRepository.DeleteCommentsByPostId(id);
            CacheRepository.Delete(id, typeof(NewsPost));
            LuceneRepositoryFactory.GetRepository<NewsPost>().Delete(id);
            UnitOfWork.Save();
        }

        public int Count()
        {
            return NewsPostRepository.Count();
        }

        public int CommentsCount(int id)
        {
            return CommentRepository.GetCountByPostId(id);
        }

        public void UpdateLucene()
        {
            LuceneRepositoryFactory.GetRepository<NewsPost>().DeleteAll();
            LuceneRepositoryFactory.GetRepository<NewsPost>().Optimize();
            var items = NewsPostRepository.GetAll();
            LuceneRepositoryFactory.GetRepository<NewsPost>().SaveOrUpdate(items);
        }
    }
}
