using ISMNewsPortal.BLL;
using ISMNewsPortal.BLL.Repositories;
using ISMNewsPortal.DAL_XML;
using ISMNewsPortal.DAL_XML.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace ISMNewsPortal.ConnectionBuilders
{
    public class XMLConnection : ConnectionBuilder
    {
        private XMLContex xmlContex;
        public XMLConnection()
        {
            string path = Path.Combine(HttpRuntime.AppDomainAppPath, "App_Data", "datebase.xml");
            xmlContex = new XMLContex(path);
        }
        public override void CreateRepositories()
        {
            var adminRepository = new AdminRepository(xmlContex);
            var commentRepository = new CommentRepository(xmlContex);
            var newsPostRepository = new NewsPostRepository(xmlContex);
            var fileRepository = new FileRepository(xmlContex);

            SessionManager.SetRepositories(adminRepository, commentRepository, newsPostRepository, fileRepository);
        }

        public override IUnitOfWork GetUnitOfWork()
        {
            return new XMLUnitOfWork(xmlContex);
        }
    }
}