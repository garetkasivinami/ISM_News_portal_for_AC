using ISMNewsPortal.DAL.Models;
using ISMNewsPortal.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISMNewsPortal.DAL.Interfaces
{
    public interface IUnitOfWork
    {
        IRepository<Admin> Admins { get; }
        IRepository<Comment> Comments { get; }
        IRepository<NewsPost> NewsPosts { get; }
        IRepository<FileModel> Files { get; }
    }
}
