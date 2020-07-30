using ISMNewsPortal.BLL.Models;
using ISMNewsPortal.BLL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISMNEWSPORTAL.DAL_XML.Repositories
{
    public class XMLUnitOfWork : IUnitOfWork
    {
        public IAdminRepository Admins => throw new NotImplementedException();

        public ICommentRepository Comments => throw new NotImplementedException();

        public INewsPostRepository NewsPosts => throw new NotImplementedException();

        public IFileRepository Files => throw new NotImplementedException();

        public int Create<T>(T item) where T : Model
        {
            throw new NotImplementedException();
        }

        public void Delete<T>(int id) where T : Model
        {
            throw new NotImplementedException();
        }

        public void Delete<T>(T item) where T : Model
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public void Update<T>(T item) where T : Model
        {
            throw new NotImplementedException();
        }
    }
}
