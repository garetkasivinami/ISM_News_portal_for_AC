using ISMNewsPortal.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
