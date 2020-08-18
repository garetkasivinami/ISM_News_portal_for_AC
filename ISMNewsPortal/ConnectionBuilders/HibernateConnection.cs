using ISMNewsPortal.BLL;
using ISMNewsPortal.BLL.Repositories;
using ISMNewsPortal.DAL;
using ISMNewsPortal.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISMNewsPortal.ConnectionBuilders
{
    public class HibernateConnection : ConnectionBuilder
    {
        public override void CreateRepositories()
        {
            var adminRepository = new AdminRepository();
            var commentRepository = new CommentRepository();
            var newsPostRepository = new NewsPostRepository();
            var fileRepository = new FileRepository();

            SessionManager.SetRepositories(adminRepository, commentRepository, newsPostRepository, fileRepository);
        }

        public override IUnitOfWork GetUnitOfWork()
        {
            return new HibernateUnitOfWork();
        }
    }
}