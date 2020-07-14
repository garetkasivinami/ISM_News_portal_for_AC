using NHibernate;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ISMNewsPortal.Models
{
    public class FileModel
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string HashCode { get; set; }

        public FileModel()
        {

        }

        public FileModel(string fileName, string fileHashCode)
        {
            Name = fileName;
            HashCode = fileHashCode;
        }
    }
}