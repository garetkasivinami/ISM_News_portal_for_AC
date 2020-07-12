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
        AdminRepository Admins { get; }
        CommentRepository Comments { get; }
        NewsPostRepository NewsPosts { get; }
        FileRepository Files { get; }
    }
}
