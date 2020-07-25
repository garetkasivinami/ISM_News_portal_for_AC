using ISMNewsPortal.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISMNewsPortal.BLL.Interfaces
{
    public interface ICommentRepository : IRepository<Comment>
    {
        IEnumerable<Comment> GetByUserName(string userName);
        IEnumerable<Comment> GetByPostId(int id);
        IEnumerable<Comment> GetByUserNameAndPostId(string userName, int postId);
        int CountByPostId(int id);
        void DeleteCommentsByPostId(int postId);
    }
}
