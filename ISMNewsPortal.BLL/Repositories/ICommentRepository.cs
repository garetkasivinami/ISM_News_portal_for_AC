using ISMNewsPortal.BLL.Models;
using System.Collections.Generic;

namespace ISMNewsPortal.BLL.Repositories
{
    public interface ICommentRepository : IRepository<Comment>
    {
        IEnumerable<Comment> GetByUserName(string userName);
        IEnumerable<Comment> GetByPostId(int id);
        IEnumerable<Comment> GetByUserNameAndPostId(string userName, int postId);
        int GetCountByPostId(int id);
        void DeleteCommentsByPostId(int postId);
    }
}
