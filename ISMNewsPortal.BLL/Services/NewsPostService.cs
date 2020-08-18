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

        public static IEnumerable<NewsPost> SortBy(IEnumerable<NewsPost> items, string sortType)
        {
            sortType = sortType?.ToLower();
            switch (sortType)
            {
                case "id":
                    items = items.OrderBy(u => u.Id);
                    break;
                case "name":
                    items = items.OrderBy(u => u.Name);
                    break;
                case "description":
                    items = items.OrderBy(u => u.Description);
                    break;
                case "editdate":
                    items = items.OrderBy(u => u.EditDate);
                    break;
                case "author":
                    items = items.OrderBy(u => u.AuthorId);
                    break;
                case "publicationdate":
                    items = items.OrderBy(u => u.PublicationDate);
                    break;
                case "visibility":
                    items = items.OrderBy(u => u.IsVisible);
                    break;
                default:
                    items = items.OrderBy(u => u.CreatedDate);
                    break;
            }
            return items;
        }

        public static IEnumerable<NewsPost> SortByReversed(IEnumerable<NewsPost> items, string sortType)
        {
            sortType = sortType?.ToLower();
            switch (sortType)
            {
                case "id":
                    items = items.OrderByDescending(u => u.Id);
                    break;
                case "name":
                    items = items.OrderByDescending(u => u.Name);
                    break;
                case "description":
                    items = items.OrderByDescending(u => u.Description);
                    break;
                case "editdate":
                    items = items.OrderByDescending(u => u.EditDate);
                    break;
                case "author":
                    items = items.OrderByDescending(u => u.AuthorId);
                    break;
                case "publicationdate":
                    items = items.OrderByDescending(u => u.PublicationDate);
                    break;
                case "visibility":
                    items = items.OrderByDescending(u => u.IsVisible);
                    break;
                default:
                    items = items.OrderByDescending(u => u.CreatedDate);
                    break;
            }
            return items;
        }
    }
}
