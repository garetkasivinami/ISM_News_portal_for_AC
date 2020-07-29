using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISMNewsPortal.BLL.Models;
using ISMNewsPortal.BLL.Repositories;
using NHibernate;

namespace ISMNewsPortal.DAL.Repositories
{
    public class HibernateUnitOfWork : IUnitOfWork
    {
        private Lazy<AdminRepository> adminRepository;
        private Lazy<CommentRepository> commentRepository;
        private Lazy<NewsPostRepository> newsPostRepository;
        private Lazy<FileRepository> fileRepository;

        private bool disposed;

        private ISession session;

        public HibernateUnitOfWork(ISession session)
        {
            this.session = session;

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

        public void Dispose()
        {
            if (!disposed)
            {
                session.Close();
                disposed = true;
            }
        }

        public void Save()
        {
            using (ITransaction transaction = session.BeginTransaction())
            {
                transaction.Commit();
            }
        }

        public void Update<T>(T item) where T : Model
        {
            session.Update(item);
        }

        public int Create<T>(T item) where T : Model
        {
            session.Save(item);
            return item.Id;
        }

        public void Delete<T>(int id)
        {
            Delete(session.Get<T>(id));
        }

        public void Delete<T>(T item)
        {
            session.Delete(item);
        }
    }
}
