using AutoMapper;
using ISMNewsPortal.BLL.BusinessModels;
using ISMNewsPortal.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISMNewsPortal.BLL.Exceptions;
using static ISMNewsPortal.BLL.UnitOfWorkManager;

namespace ISMNewsPortal.BLL.Services
{
    public class CommentService
    {

        public IEnumerable<Comment> GetComments()
        {
            return UnitOfWork.Comments.GetAll();
        }

        public Comment GetComment(int id)
        {
            var comment = UnitOfWork.Comments.Get(id);
            if (comment == null)
                throw new CommentNullException();
            return comment;
        }

        public IEnumerable<Comment> GetCommentsByPostId(int id)
        {
            var comments = UnitOfWork.Comments.GetByPostId(id).Reverse();
            return comments;
        }

        public void UpdateComment(Comment commentDTO)
        {
            UnitOfWork.Update(commentDTO);
            UnitOfWork.Save();
        }

        public int CreateComment(Comment commentDTO)
        {
            int id = UnitOfWork.Create(commentDTO);
            UnitOfWork.Save();
            return id;
        }

        public IEnumerable<Comment> GetCommentsWithTools(OptionsCollectionById options)
        {
            var comments = UnitOfWork.Comments.GetWithOptions(options);
            return comments;
        }

        public void DeleteComment(int id)
        {
            UnitOfWork.Comments.Delete(id);
            UnitOfWork.Save();
        }

        public int Count()
        {
            return UnitOfWork.Comments.Count();
        }

        public int GetCommentCountByPostId(int id)
        {
            return UnitOfWork.Comments.GetCountByPostId(id);
        }
    }
}
