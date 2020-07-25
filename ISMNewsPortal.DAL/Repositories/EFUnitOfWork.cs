using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISMNewsPortal.BLL.Repositories;
using ISMNewsPortal.DAL.Models;
using NHibernate;

namespace ISMNewsPortal.DAL.Repositories
{
    public class EFUnitOfWork : IUnitOfWork, IDisposable
    {
        private Lazy<AdminRepository> adminRepository;
        private Lazy<CommentRepository> commentRepository;
        private Lazy<NewsPostRepository> newsPostRepository;
        private Lazy<FileRepository> fileRepository;

        private List<object> signedObjects;
        private bool disposed;

        private ISession session;

        public EFUnitOfWork()
        {
            session = NHibernateSession.OpenSession();

            signedObjects = new List<object>();

            adminRepository = new Lazy<AdminRepository>(() => new AdminRepository(session));
            commentRepository = new Lazy<CommentRepository>(() => new CommentRepository(session));
            newsPostRepository = new Lazy<NewsPostRepository>(() => new NewsPostRepository(session));
            fileRepository = new Lazy<FileRepository>(() => new FileRepository(session));
        }
        public IAdminRepository Admins
        {
            get => adminRepository.Value;
        }
        public ICommentRepository Comments
        {
            get => commentRepository.Value;
        }
        public INewsPostRepository NewsPosts
        {
            get => newsPostRepository.Value;
        }
        public IFileRepository Files
        {
            get => fileRepository.Value;
        }
        public List<object> SignedObjects { 
            get => signedObjects; 
        }
        public void Dispose()
        {
            if (signedObjects.Count == 0 && !disposed)
            {
                session.Close();
                disposed = true;
            }
        }
    }
}
