using ISMNewsPortal.BLL.BusinessModels;
using ISMNewsPortal.BLL.DTO;
using ISMNewsPortal.BLL.Interfaces;
using ISMNewsPortal.DAL.Models;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ISMNewsPortal.BLL.Mappers.Automapper;

namespace ISMNewsPortal.DAL.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private ISession session;

        public CommentRepository(ISession session)
        {
            this.session = session;
        }

        public int Count()
        {
            return session.Query<Comment>().Count();
        }

        public int CountByPostId(int id)
        {
            return session.Query<Comment>().Count(u => u.NewsPostId == id);
        }

        public int Create(CommentDTO item)
        {
            var comment = MapFromCommentDTO<Comment>(item);
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Save(comment);
                transaction.Commit();
                return comment.Id;
            }
        }

        public void Delete(int id)
        {
            var comment = session.Get<Comment>(id);
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Delete(comment);
                transaction.Commit();
            }
        }

        public void DeleteCommentsByPostId(int postId)
        {
            var comments = session.Query<Comment>().Where(u => u.NewsPostId == postId);
            using (ITransaction transaction = session.BeginTransaction())
            {
                foreach (Comment comment in comments)
                    session.Delete(comment);
                transaction.Commit();
            }
        }

        public CommentDTO Get(int id)
        {
            var comment = session.Get<Comment>(id);
            return MapToCommentDTO(comment);
        }

        public IEnumerable<CommentDTO> GetAll()
        {
            var comments = session.Query<Comment>();
            return MapToCommentDTOList(comments);
        }

        public IEnumerable<CommentDTO> GetAllWithTools(ToolsDTO toolBar)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<CommentDTO> GetByPostId(int id)
        {
            var comments = session.Query<Comment>().Where(u => u.NewsPostId == id);
            return MapToCommentDTOList(comments);
        }

        public IEnumerable<CommentDTO> GetByUserName(string userName)
        {
            var comments = session.Query<Comment>().Where(u => u.UserName == userName);
            return MapToCommentDTOList(comments);
        }

        public IEnumerable<CommentDTO> GetByUserNameAndPostId(string userName, int postId)
        {
            var comments = session.Query<Comment>().Where(u => u.NewsPostId == postId && u.UserName == userName);
            return MapToCommentDTOList(comments);
        }

        public void Update(CommentDTO item)
        {
            var comment = MapFromCommentDTO<Comment>(item);
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Update(comment);
                transaction.Commit();
            }
        }
    }
}
