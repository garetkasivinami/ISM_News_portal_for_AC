using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISMNewsPortal.Models
{
    public class Admin
    {
        public virtual int Id { get; set; }
        public virtual int UserId { get; set; }
        public virtual byte AccessLevel { get; set; }
    }
}