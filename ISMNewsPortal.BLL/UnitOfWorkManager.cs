using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISMNewsPortal.BLL.Repositories;

namespace ISMNewsPortal.BLL
{
    public static class UnitOfWorkManager
    {
        private static IUnitOfWork unitOfWork;
        private static IAdminRepository adminRepository;
        private static ICommentRepository commentRepository;
        private static INewsPostRepository newsPostRepository;
        private static IFileRepository fileRepository;

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

        public static void SetUnitOfWork(IUnitOfWork unitOfWork, IAdminRepository adminRepository, 
            ICommentRepository commentRepository, INewsPostRepository newsPostRepository, IFileRepository fileRepository)
        {
            UnitOfWorkManager.unitOfWork = unitOfWork;
            UnitOfWorkManager.adminRepository = adminRepository;
            UnitOfWorkManager.commentRepository = commentRepository;
            UnitOfWorkManager.newsPostRepository = newsPostRepository;
            UnitOfWorkManager.fileRepository = fileRepository;
        }
    }
}
