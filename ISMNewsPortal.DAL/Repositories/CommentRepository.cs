using ISMNewsPortal.BLL.BusinessModels;
using ISMNewsPortal.BLL.Models;
using ISMNewsPortal.BLL.Repositories;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ISMNewsPortal.BLL.Mappers.Automapper;

namespace ISMNewsPortal.DAL.Repositories
{
    public class CommentRepository : Repository<Comment>, ICommentRepository
    {

        public CommentRepository(ISession session) : base(session)
        {

        }

        public int GetCountByPostId(int id)
        {
            return _session.Query<Comment>().Count(u => u.NewsPostId == id);
        }

        public void DeleteCommentsByPostId(int postId)
        {
            var comments = _session.Query<Comment>().Where(u => u.NewsPostId == postId);
            foreach (Comment comment in comments)
                _session.Delete(comment);
        }

        public IEnumerable<Comment> GetByPostId(int id)
        {
            return _session.Query<Comment>().Where(u => u.NewsPostId == id);
        }

        public IEnumerable<Comment> GetByUserName(string userName)
        {
            return _session.Query<Comment>().Where(u => u.UserName == userName);
        }

        public IEnumerable<Comment> GetByUserNameAndPostId(string userName, int postId)
        {
            return _session.Query<Comment>().Where(u => u.NewsPostId == postId && u.UserName == userName);
        }
    }
}
