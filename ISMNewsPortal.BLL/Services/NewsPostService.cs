using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISMNewsPortal.BLL.Models;
using ISMNewsPortal.BLL.Infrastructure;
using ISMNewsPortal.BLL.BusinessModels;
using ISMNewsPortal.BLL.Exceptions;
using static ISMNewsPortal.BLL.UnitOfWorkManager;

namespace ISMNewsPortal.BLL.Services
{
    public class NewsPostService
    {

        public IEnumerable<NewsPost> GetNewsPosts()
        {
            return UnitOfWork.NewsPosts.GetAll();
        }

        public IEnumerable<NewsPost> GetNewsPostsWithTools(Options toolsDTO)
        {
            var newsPosts = UnitOfWork.NewsPosts.GetWithOptions(toolsDTO);
            toolsDTO.Pages = toolsDTO.Pages;
            return newsPosts;
        }

        public IEnumerable<NewsPost> GetNewsPostsWithAdminTools(Options toolsDTO)
        {
            toolsDTO.Admin = true;
            var newsPosts = UnitOfWork.NewsPosts.GetWithOptions(toolsDTO);
            toolsDTO.Pages = toolsDTO.Pages;
            return newsPosts;
        }

        public NewsPost GetNewsPost(int id)
        {
            var newsPost = UnitOfWork.NewsPosts.Get(id);
            if (newsPost == null)
                throw new NewsPostNullException();
            return newsPost;
        }

        public void UpdateNewsPost(NewsPost newsPostDTO)
        {
            UnitOfWork.Update(newsPostDTO);
            UnitOfWork.Save();
        }

        public void CreateNewsPost(NewsPost newsPostDTO)
        {
            UnitOfWork.Create(newsPostDTO);
            UnitOfWork.Save();
        }

        public void DeleteNewsPost(int id)
        {
            UnitOfWork.NewsPosts.Delete(id);
            UnitOfWork.Comments.DeleteCommentsByPostId(id);
            UnitOfWork.Save();
        }

        public int Count()
        {
            return UnitOfWork.NewsPosts.Count();
        }

        public int CommentsCount(int id)
        {
            return UnitOfWork.Comments.GetCountByPostId(id);
        }
    }
}
