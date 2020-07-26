using ISMNewsPortal.BLL.BusinessModels;
using ISMNewsPortal.BLL.Models;
using ISMNewsPortal.BLL.Repositories;
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

        public int GetCountByPostId(int id)
        {
            return session.Query<Comment>().Count(u => u.NewsPostId == id);
        }

        public int Create(Comment item)
        {
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Save(item);
                transaction.Commit();
                return item.Id;
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

        public Comment Get(int id)
        {
            return session.Get<Comment>(id);
        }

        public IEnumerable<Comment> GetAll()
        {
            return session.Query<Comment>();
        }

        public IEnumerable<Comment> GetWithOptions(ToolsDTO toolBar)
        {
            return GetAll();
        }

        public IEnumerable<Comment> GetByPostId(int id)
        {
            return session.Query<Comment>().Where(u => u.NewsPostId == id);
        }

        public IEnumerable<Comment> GetByUserName(string userName)
        {
            return session.Query<Comment>().Where(u => u.UserName == userName);
        }

        public IEnumerable<Comment> GetByUserNameAndPostId(string userName, int postId)
        {
            return session.Query<Comment>().Where(u => u.NewsPostId == postId && u.UserName == userName);
        }

        public void Update(Comment item)
        {
            var comment = item;
            var createdComment = session.Get<Comment>(item.Id);
            Map(comment, createdComment);
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Update(createdComment);
                transaction.Commit();
            }
        }
    }
}
