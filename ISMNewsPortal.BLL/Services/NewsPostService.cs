using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISMNewsPortal.BLL.Models;
using ISMNewsPortal.BLL.Mappers;
using ISMNewsPortal.BLL.Infrastructure;
using ISMNewsPortal.BLL.BusinessModels;

namespace ISMNewsPortal.BLL.Services
{
    public class NewsPostService
    {

        public IEnumerable<NewsPost> GetNewsPosts()
        {
            return Unity.UnitOfWork.NewsPosts.GetAll();
        }

        public IEnumerable<NewsPost> GetNewsPostsWithTools(ToolsDTO toolsDTO)
        {
            var newsPosts = Unity.UnitOfWork.NewsPosts.GetWithOptions(toolsDTO);
            toolsDTO.Pages = toolsDTO.Pages;
            return newsPosts;
        }

        public IEnumerable<NewsPost> GetNewsPostsWithAdminTools(ToolsDTO toolsDTO)
        {
            toolsDTO.Admin = true;
            var newsPosts = Unity.UnitOfWork.NewsPosts.GetWithOptions(toolsDTO);
            toolsDTO.Pages = toolsDTO.Pages;
            return newsPosts;
        }

        public NewsPost GetNewsPost(int id)
        {
            var newsPost = Unity.UnitOfWork.NewsPosts.Get(id);
            if (newsPost == null)
                throw new Exception("News post is null");
            return newsPost;
        }

        public void UpdateNewsPost(NewsPost newsPostDTO)
        {
            Unity.UnitOfWork.NewsPosts.Update(newsPostDTO);
        }

        public void CreateNewsPost(NewsPost newsPostDTO)
        {
            Unity.UnitOfWork.NewsPosts.Create(newsPostDTO);
        }

        public void DeleteNewsPost(int id)
        {
            Unity.UnitOfWork.NewsPosts.Delete(id);
            Unity.UnitOfWork.Comments.DeleteCommentsByPostId(id);
        }

        public int Count()
        {
            return Unity.UnitOfWork.NewsPosts.Count();
        }

        public int CommentsCount(int id)
        {
            return Unity.UnitOfWork.Comments.GetCountByPostId(id);
        }
    }
}
