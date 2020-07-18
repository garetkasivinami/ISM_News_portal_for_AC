using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISMNewsPortal.BLL.DTO;
using ISMNewsPortal.BLL.Mappers;
using ISMNewsPortal.BLL.Infrastructure;
using ISMNewsPortal.BLL.BusinessModels;

namespace ISMNewsPortal.BLL.Services
{
    public class NewsPostService
    {

        public IEnumerable<NewsPostDTO> GetNewsPosts()
        {
            return Unity.UnitOfWork.NewsPosts.GetAll();
        }

        public IEnumerable<NewsPostDTO> GetNewsPostsWithTools(ToolsDTO toolsDTO)
        {
            var newsPosts = Unity.UnitOfWork.NewsPosts.GetAllWithTools(toolsDTO);
            toolsDTO.Pages = toolsDTO.Pages;
            return newsPosts;
        }

        public IEnumerable<NewsPostDTO> GetNewsPostsWithAdminTools(ToolsDTO toolsDTO)
        {
            toolsDTO.Admin = true;
            var newsPosts = Unity.UnitOfWork.NewsPosts.GetAllWithTools(toolsDTO);
            toolsDTO.Pages = toolsDTO.Pages;
            return newsPosts;
        }

        public NewsPostDTO GetNewsPost(int id)
        {
            var newsPost = Unity.UnitOfWork.NewsPosts.Get(id);
            if (newsPost == null)
                throw new Exception("News post is null");
            return newsPost;
        }

        public void UpdateNewsPost(NewsPostDTO newsPostDTO)
        {
            Unity.UnitOfWork.NewsPosts.Update(newsPostDTO);
        }

        public void CreateNewsPost(NewsPostDTO newsPostDTO)
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
            return Unity.UnitOfWork.Comments.CountByPostId(id);
        }
    }
}
