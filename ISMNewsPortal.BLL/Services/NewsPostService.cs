using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISMNewsPortal.BLL.DTO;
using ISMNewsPortal.DAL.Repositories;
using ISMNewsPortal.DAL.Models;
using ISMNewsPortal.BLL.Mappers;
using ISMNewsPortal.BLL.Infrastructure;
using ISMNewsPortal.BLL.BusinessModels;

namespace ISMNewsPortal.BLL.Services
{
    public class NewsPostService : Service
    {
        public NewsPostService() : base()
        {

        }

        public NewsPostService(Service service) : base(service)
        {

        }

        public IEnumerable<NewsPostDTO> GetNewsPosts()
        {
            return DTOMapper.NewsPostMapperToDTO.Map<IEnumerable<NewsPost>, List<NewsPostDTO>>(database.NewsPosts.GetAll());
        }

        public IEnumerable<NewsPostDTO> GetNewsPostsWithTools(ToolsDTO toolsDTO)
        {
            var toolsModel = DTOMapper.ToolsMapper.Map<ToolsDTO, ToolBarModel>(toolsDTO);
            var newsPosts = database.NewsPosts.GetAllWithTools(toolsModel);
            return DTOMapper.NewsPostMapperToDTO.Map<IEnumerable<NewsPost>, List<NewsPostDTO>>(newsPosts);
        }

        public IEnumerable<NewsPostDTO> GetNewsPostsWithAdminTools(ToolsDTO toolsDTO)
        {
            var toolsModel = DTOMapper.ToolsMapper.Map<ToolsDTO, ToolBarModel>(toolsDTO);
            var newsPosts = database.NewsPosts.GetAllWithAdminTools(toolsModel);
            return DTOMapper.NewsPostMapperToDTO.Map<IEnumerable<NewsPost>, List<NewsPostDTO>>(newsPosts);
        }

        public NewsPostDTO GetNewsPost(int id)
        {
            var newsPost = database.NewsPosts.Get(id);
            if (newsPost == null)
                throw new ValidationException("News post is null", "");
            return DTOMapper.NewsPostMapperToDTO.Map<NewsPost, NewsPostDTO>(newsPost);
        }

        public IEnumerable<NewsPostDTO> FindNewsPosts(Func<NewsPost, bool> predicate)
        {
            var newsPosts = database.NewsPosts.Find(predicate);
            return DTOMapper.NewsPostMapperToDTO.Map<IEnumerable<NewsPost>, List<NewsPostDTO>>(newsPosts);
        }

        public void UpdateNewsPost(NewsPostDTO newsPostDTO)
        {
            var newsPost = DTOMapper.NewsPostMapper.Map<NewsPostDTO, NewsPost>(newsPostDTO);
            database.NewsPosts.Update(newsPost);
        }

        public void CreateNewsPost(NewsPostDTO newsPostDTO)
        {
            var newsPost = DTOMapper.NewsPostMapper.Map<NewsPostDTO, NewsPost>(newsPostDTO);
            database.NewsPosts.Create(newsPost);
        }

        public void DeleteNewsPost(int id)
        {
            database.NewsPosts.Delete(id);
            var comments = database.Comments.Find(u => u.NewsPostId == id);
            foreach(Comment comment in comments)
            {
                database.Comments.Delete(comment.Id);
            }
        }

        public int Count()
        {
            return database.NewsPosts.Count();
        }

        public int Count(Func<NewsPost, bool> predicate)
        {
            return database.NewsPosts.Count(predicate);
        }

        public int CommentsCount(int id)
        {
            return database.Comments.Count(u => u.NewsPostId == id);
        }
    }
}
