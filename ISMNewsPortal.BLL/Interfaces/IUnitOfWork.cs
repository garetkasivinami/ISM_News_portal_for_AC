using ISMNewsPortal.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISMNewsPortal.BLL.Interfaces
{
    public interface IUnitOfWork
    {
        IAdminRepository Admins { get; }
        ICommentRepository Comments { get; }
        INewsPostRepository NewsPosts { get; }
        IFileRepository Files { get; }
    }
}
