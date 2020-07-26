using AutoMapper;
using ISMNewsPortal.BLL.BusinessModels;
using ISMNewsPortal.BLL.Models;
using ISMNewsPortal.BLL.Infrastructure;
using ISMNewsPortal.BLL.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISMNewsPortal.BLL.Exceptions;

namespace ISMNewsPortal.BLL.Services
{
    public class CommentService
    {

        public IEnumerable<Comment> GetComments()
        {
            return UnitOfWorkManager.UnitOfWork.Comments.GetAll();
        }

        public Comment GetComment(int id)
        {
            var comment = UnitOfWorkManager.UnitOfWork.Comments.Get(id);
            if (comment == null)
                throw new CommentNullException();
            return comment;
        }

        public IEnumerable<Comment> GetCommentsByPostId(int id)
        {
            var comments = UnitOfWorkManager.UnitOfWork.Comments.GetByPostId(id).Reverse();
            return comments;
        }

        public void UpdateComment(Comment commentDTO)
        {
            UnitOfWorkManager.UnitOfWork.Update(commentDTO);
        }

        public int CreateComment(Comment commentDTO)
        {
            return UnitOfWorkManager.UnitOfWork.Create(commentDTO);
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
            UnitOfWorkManager.UnitOfWork.Comments.Delete(id);
        }

        public int Count()
        {
            return UnitOfWorkManager.UnitOfWork.Comments.Count();
        }

        public int GetCommentCountByPostId(int id)
        {
            return UnitOfWorkManager.UnitOfWork.Comments.GetCountByPostId(id);
        }
    }
}
