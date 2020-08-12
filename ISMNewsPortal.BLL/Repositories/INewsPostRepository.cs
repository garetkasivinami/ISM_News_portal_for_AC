using ISMNewsPortal.BLL.Models;
using System.Collections.Generic;

namespace ISMNewsPortal.BLL.Repositories
{
    public interface INewsPostRepository : IRepository<NewsPost>
    {
        IEnumerable<NewsPost> GetByName(string name);
        IEnumerable<NewsPost> GetByAuthorId(int id);
        IEnumerable<NewsPost> GetByVisibility(bool visible);
        IEnumerable<NewsPost> GetByImageId(int id);
        int GetCommentsCount(int postId);
    }
}
