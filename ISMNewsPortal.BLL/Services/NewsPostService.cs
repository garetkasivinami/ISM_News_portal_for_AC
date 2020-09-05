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
using System.ComponentModel;

namespace ISMNewsPortal.BLL.Services
{
    public class NewsPostService
    {
        private const string NewsPostCache = "newspost";
        private const string NewsPostToolsCache = "newspost-tools";

        public IEnumerable<NewsPost> GetNewsPostsWithTools(Options options)
        {
            IEnumerable<NewsPost> newsPosts;
            if (string.IsNullOrEmpty(options.Search))
            {
                newsPosts = CacheRepository.GetItems<NewsPost>(GetOptionsString(options));

                if (newsPosts != null)
                    return newsPosts;

                newsPosts = NewsPostRepository.GetWithOptions(options);
                CacheRepository.Add(newsPosts, GetOptionsString(options));
            }
            else
            {
                newsPosts = LuceneRepositoryFactory.GetRepository<NewsPost>().Search(options);
            }
            return newsPosts;
        }

        public IEnumerable<NewsPost> GetNewsPostsWithAdminTools(Options options)
        {
            options.Admin = true;
            return GetNewsPostsWithTools(options);
        }

        public NewsPost GetNewsPost(int id)
        {
            var newsPost = CacheRepository.GetItem<NewsPost>($"{NewsPostCache}-{id}");
            if (newsPost != null)
                return newsPost as NewsPost;

            newsPost = NewsPostRepository.Get(id);
            if (newsPost == null)
                throw new NewsPostNullException();
            CacheRepository.Add(newsPost, $"{NewsPostCache}-{id}");
            return newsPost as NewsPost;
        }

        public void UpdateNewsPost(NewsPost newsPost)
        {
            NewsPostRepository.Update(newsPost);
            CacheRepository.Update(newsPost, $"{NewsPostCache}-{newsPost.Id}");
            CacheRepository.DeleteByPartOfTheKey(NewsPostToolsCache);
            LuceneRepositoryFactory.GetRepository<NewsPost>().SaveOrUpdate(newsPost);
            UnitOfWork.Save();
        }

        public void CreateNewsPost(NewsPost newsPost)
        {
            NewsPostRepository.Create(newsPost);
            CacheRepository.Add(newsPost, $"{NewsPostCache}-{newsPost.Id}");
            CacheRepository.DeleteByPartOfTheKey(NewsPostToolsCache);
            LuceneRepositoryFactory.GetRepository<NewsPost>().SaveOrUpdate(newsPost);
            UnitOfWork.Save();
        }

        public void DeleteNewsPost(int id)
        {
            NewsPostRepository.Delete(id);
            CommentRepository.DeleteCommentsByPostId(id);
            CacheRepository.Delete($"{NewsPostCache}-{id}");
            CacheRepository.DeleteByPartOfTheKey(NewsPostToolsCache);
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

        private string GetOptionsString(Options options)
        {
            return $"{NewsPostToolsCache}-{options.DateRange}-{options.Search}-{options.SortType}-{options.Page}-{options.Admin}-{options.Published}";
        }
    }
}
