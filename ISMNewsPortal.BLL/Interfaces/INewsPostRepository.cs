using ISMNewsPortal.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISMNewsPortal.BLL.Interfaces
{
    public interface INewsPostRepository : IRepository<NewsPost>
    {
        IEnumerable<NewsPost> GetByName(string name);
        IEnumerable<NewsPost> GetByAuthorId(int id);
        IEnumerable<NewsPost> GetByVisibility(bool visible);
        IEnumerable<NewsPost> GetByImageId(int id);
        int CommentsCount(int postId);
    }
}
