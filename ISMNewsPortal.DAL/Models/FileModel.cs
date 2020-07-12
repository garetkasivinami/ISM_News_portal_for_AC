using NHibernate;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ISMNewsPortal.DAL.Models
{
    public class FileModel
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string HashCode { get; set; }
    }
}