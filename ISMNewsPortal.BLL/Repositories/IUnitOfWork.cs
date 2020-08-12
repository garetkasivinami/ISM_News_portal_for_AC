using System;

namespace ISMNewsPortal.BLL.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        void Save();
    }
}
