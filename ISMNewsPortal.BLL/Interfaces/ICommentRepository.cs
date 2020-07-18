using ISMNewsPortal.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISMNewsPortal.BLL.Interfaces
{
    public interface ICommentRepository : IRepository<CommentDTO>
    {
        IEnumerable<CommentDTO> GetByUserName(string userName);
        IEnumerable<CommentDTO> GetByPostId(int id);
        IEnumerable<CommentDTO> GetByUserNameAndPostId(string userName, int postId);
        int CountByPostId(int id);
        void DeleteCommentsByPostId(int postId);
    }
}
