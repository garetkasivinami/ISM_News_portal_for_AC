using ISMNewsPortal.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISMNewsPortal.BLL.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IAdminRepository Admins { get; }
        ICommentRepository Comments { get; }
        INewsPostRepository NewsPosts { get; }
        IFileRepository Files { get; }
        void Save();
    }
}
