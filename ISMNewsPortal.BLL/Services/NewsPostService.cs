using System.Collections.Generic;
using System.Linq;
using ISMNewsPortal.BLL.Models;
using ISMNewsPortal.BLL.BusinessModels;
using ISMNewsPortal.BLL.Exceptions;
using static ISMNewsPortal.BLL.SessionManager;

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
            var newsPosts = NewsPostRepository.GetWithOptions(options);
            return newsPosts;
        }

        public IEnumerable<NewsPost> GetNewsPostsWithAdminTools(Options options)
        {
            options.Admin = true;
            var newsPosts = NewsPostRepository.GetWithOptions(options);
            options.Pages = options.Pages;
            return newsPosts;
        }

        public NewsPost GetNewsPost(int id)
        {
            var newsPost = NewsPostRepository.Get(id);
            if (newsPost == null)
                throw new NewsPostNullException();
            return newsPost;
        }

        public void UpdateNewsPost(NewsPost newsPost)
        {
            NewsPostRepository.Update(newsPost);
            UnitOfWork.Save();
        }

        public void CreateNewsPost(NewsPost newsPost)
        {
            NewsPostRepository.Create(newsPost);
            UnitOfWork.Save();
        }

        public void DeleteNewsPost(int id)
        {
            NewsPostRepository.Delete(id);
            CommentRepository.DeleteCommentsByPostId(id);
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
    }
}
