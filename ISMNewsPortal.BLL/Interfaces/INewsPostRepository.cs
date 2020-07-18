using ISMNewsPortal.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISMNewsPortal.BLL.Interfaces
{
    public interface INewsPostRepository : IRepository<NewsPostDTO>
    {
        IEnumerable<NewsPostDTO> GetByName(string name);
        IEnumerable<NewsPostDTO> GetByAuthorId(int id);
        IEnumerable<NewsPostDTO> GetByVisibility(bool visible);
        IEnumerable<NewsPostDTO> GetByImageId(int id);
        int CommentsCount(int postId);
    }
}
