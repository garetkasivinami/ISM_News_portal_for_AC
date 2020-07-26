using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISMNewsPortal.BLL.Models;
using ISMNewsPortal.BLL.Mappers;
using ISMNewsPortal.BLL.Infrastructure;
using ISMNewsPortal.BLL.BusinessModels;
using ISMNewsPortal.BLL.Exceptions;

namespace ISMNewsPortal.BLL.Services
{
    public class NewsPostService
    {

        public IEnumerable<NewsPost> GetNewsPosts()
        {
            return UnitOfWorkManager.UnitOfWork.NewsPosts.GetAll();
        }

        public IEnumerable<NewsPost> GetNewsPostsWithTools(Options toolsDTO)
        {
            var newsPosts = UnitOfWorkManager.UnitOfWork.NewsPosts.GetWithOptions(toolsDTO);
            toolsDTO.Pages = toolsDTO.Pages;
            return newsPosts;
        }

        public IEnumerable<NewsPost> GetNewsPostsWithAdminTools(Options toolsDTO)
        {
            toolsDTO.Admin = true;
            var newsPosts = UnitOfWorkManager.UnitOfWork.NewsPosts.GetWithOptions(toolsDTO);
            toolsDTO.Pages = toolsDTO.Pages;
            return newsPosts;
        }

        public NewsPost GetNewsPost(int id)
        {
            var newsPost = UnitOfWorkManager.UnitOfWork.NewsPosts.Get(id);
            if (newsPost == null)
                throw new NewsPostNullException();
            return newsPost;
        }

        public void UpdateNewsPost(NewsPost newsPostDTO)
        {
            UnitOfWorkManager.UnitOfWork.Update(newsPostDTO);
        }

        public void CreateNewsPost(NewsPost newsPostDTO)
        {
            UnitOfWorkManager.UnitOfWork.Create(newsPostDTO);
        }

        public void DeleteNewsPost(int id)
        {
            UnitOfWorkManager.UnitOfWork.NewsPosts.Delete(id);
            UnitOfWorkManager.UnitOfWork.Comments.DeleteCommentsByPostId(id);
        }

        public int Count()
        {
            return UnitOfWorkManager.UnitOfWork.NewsPosts.Count();
        }

        public int CommentsCount(int id)
        {
            return UnitOfWorkManager.UnitOfWork.Comments.GetCountByPostId(id);
        }
    }
}
