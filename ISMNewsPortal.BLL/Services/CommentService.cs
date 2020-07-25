using AutoMapper;
using ISMNewsPortal.BLL.BusinessModels;
using ISMNewsPortal.BLL.DTO;
using ISMNewsPortal.BLL.Infrastructure;
using ISMNewsPortal.BLL.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISMNewsPortal.BLL.Services
{
    public class CommentService
    {

        public IEnumerable<Comment> GetComments()
        {
            return Unity.UnitOfWork.Comments.GetAll();
        }

        public Comment GetComment(int id)
        {
            var comment = Unity.UnitOfWork.Comments.Get(id);
            if (comment == null)
                throw new Exception("Comment is null");
            return comment;
        }

        public IEnumerable<Comment> GetCommentsByPostId(int id)
        {
            var comments = Unity.UnitOfWork.Comments.GetByPostId(id).Reverse();
            return comments;
        }

        public void UpdateComment(Comment commentDTO)
        {
            Unity.UnitOfWork.Comments.Update(commentDTO);
        }

        public int CreateComment(Comment commentDTO)
        {
            return Unity.UnitOfWork.Comments.Create(commentDTO);
        }

        //public IEnumerable<CommentDTO> GetCommentsWithTools(ToolsDTO toolsDTO)
        //{
        //    var toolsModel = DTOMapper.ToolsMapper.Map<ToolsDTO, ToolBarModel>(toolsDTO);
        //    var comments = database.Comments.GetAllWithTools(toolsModel);
        //    toolsDTO.Pages = toolsModel.Pages;
        //    return comments;
        //}

        public void DeleteComment(int id)
        {
            Unity.UnitOfWork.Comments.Delete(id);
        }

        public int Count()
        {
            return Unity.UnitOfWork.Comments.Count();
        }

        public int GetCommentCountByPostId(int id)
        {
            return Unity.UnitOfWork.Comments.CountByPostId(id);
        }
    }
}
