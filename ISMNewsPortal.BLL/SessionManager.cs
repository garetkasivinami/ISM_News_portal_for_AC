using ISMNewsPortal.BLL.Repositories;
using ISMNewsPortal.BLL.Lucene;
using System.Runtime.CompilerServices;
using System.Web;
using ISMNewsPortal.BLL.Models;

namespace ISMNewsPortal.BLL
{
    public static class SessionManager
    {
        private static IUnitOfWork unitOfWork;
        private static IAdminRepository adminRepository;
        private static ICommentRepository commentRepository;
        private static INewsPostRepository newsPostRepository;
        private static IFileRepository fileRepository;
        private static ILuceneRepositoryFactory luceneRepositoryFactory;
        private static ICacheRepository cacheRepository;

        public static IUnitOfWork UnitOfWork 
        {
            get => unitOfWork;
        }
        public static IAdminRepository AdminRepository { 
            get => adminRepository;
        }
        public static ICommentRepository CommentRepository { 
            get => commentRepository;
        }
        public static INewsPostRepository NewsPostRepository { 
            get => newsPostRepository;
        }
        public static IFileRepository FileRepository { 
            get => fileRepository;
        }
        public static ILuceneRepositoryFactory LuceneRepositoryFactory
        {
            get => luceneRepositoryFactory;
        }

        public static ICacheRepository CacheRepository
        {
            get => cacheRepository;
        }

        public static void SetRepositories(IAdminRepository adminRepository, 
            ICommentRepository commentRepository, INewsPostRepository newsPostRepository, IFileRepository fileRepository)
        {
            SessionManager.adminRepository = adminRepository;
            SessionManager.commentRepository = commentRepository;
            SessionManager.newsPostRepository = newsPostRepository;
            SessionManager.fileRepository = fileRepository;
        }

        public static void SetLuceneRepositoryFactory(ILuceneRepositoryFactory luceneRepositoryFactory)
        {
            SessionManager.luceneRepositoryFactory = luceneRepositoryFactory;
        }

        public static void SetUnitOfWork(IUnitOfWork unitOfWork)
        {
            SessionManager.unitOfWork = unitOfWork;
        }

        public static void SetCacheRepository(ICacheRepository cacheRepository)
        {
            SessionManager.cacheRepository = cacheRepository;
        }
    }
}
