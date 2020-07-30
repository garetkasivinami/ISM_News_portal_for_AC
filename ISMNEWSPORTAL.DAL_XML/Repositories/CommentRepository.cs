using ISMNewsPortal.BLL.Models;
using ISMNewsPortal.BLL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISMNEWSPORTAL.DAL_XML.Repositories
{
    public class CommentRepository : Repository<Comment>, ICommentRepository
    {
        public CommentRepository(XMLContex contex) : base(contex)
        {

        }

        public void DeleteCommentsByPostId(int postId)
        {
            var comments = GetAll().Where(u => u.NewsPostId == postId);
            foreach (Comment comment in comments)
                Delete(comment.Id);
        }

        public IEnumerable<Comment> GetByPostId(int id)
        {
            return GetAll().Where(u => u.NewsPostId == id);
        }

        public IEnumerable<Comment> GetByUserName(string userName)
        {
            return GetAll().Where(u => u.UserName == userName);
        }

        public IEnumerable<Comment> GetByUserNameAndPostId(string userName, int postId)
        {
            return GetAll().Where(u => u.UserName == userName && u.NewsPostId == postId);
        }

        public int GetCountByPostId(int id)
        {
            return GetAll().Where(u => u.NewsPostId == id).Count();
        }
    }
}
