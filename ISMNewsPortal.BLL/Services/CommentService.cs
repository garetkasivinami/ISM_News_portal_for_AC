using AutoMapper;
using ISMNewsPortal.BLL.BusinessModels;
using ISMNewsPortal.BLL.DTO;
using ISMNewsPortal.BLL.Infrastructure;
using ISMNewsPortal.BLL.Mappers;
using ISMNewsPortal.DAL.Models;
using ISMNewsPortal.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISMNewsPortal.BLL.Services
{
    public class CommentService : Service
    {
        public CommentService() : base()
        {

        }

        public CommentService(Service service) : base(service)
        {

        }

        public IEnumerable<CommentDTO> GetComments()
        {
            return DTOMapper.CommentMapperToDTO.Map<IEnumerable<Comment>, List<CommentDTO>>(database.Comments.GetAll());
        }

        public CommentDTO GetComment(int id)
        {
            var comment = database.Comments.Get(id);
            if (comment == null)
                throw new ValidationException("Comment is null", "");
            return DTOMapper.CommentMapperToDTO.Map<Comment, CommentDTO>(comment);
        }

        public IEnumerable<CommentDTO> GetCommentsByPostId(int id)
        {
            var comments = database.Comments.Find(u => u.NewsPostId == id);
            return DTOMapper.CommentMapperToDTO.Map<IEnumerable<Comment>, List<CommentDTO>>(comments);
        }

        public void UpdateComment(CommentDTO commentDTO)
        {
            var comment = DTOMapper.CommentMapper.Map<CommentDTO, Comment>(commentDTO);
            database.Comments.Update(comment);
        }

        public void CreateComment(CommentDTO commentDTO)
        {
            var comment = DTOMapper.CommentMapper.Map<CommentDTO, Comment>(commentDTO);
            database.Comments.Create(comment);
        }

        public IEnumerable<CommentDTO> GetCommentsWithTools(ToolsDTO toolsDTO)
        {
            var toolsModel = DTOMapper.ToolsMapper.Map<ToolsDTO, ToolBarModel>(toolsDTO);
            var comments = database.Comments.GetAllWithTools(toolsModel);
            return DTOMapper.CommentMapperToDTO.Map<IEnumerable<Comment>, List<CommentDTO>>(comments);
        }


        public void DeleteComment(int id)
        {
            database.Comments.Delete(id);
        }

        public int Count()
        {
            return database.Comments.Count();
        }

        public int GetCommentCountByPostId(int id)
        {
            return database.Comments.Count(u => u.NewsPostId == id);
        }
    }
}
