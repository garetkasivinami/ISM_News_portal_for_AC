using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISMNewsPortal.Models
{
    public class UserLike
    {
        public virtual int Id { get; set; }
        public virtual Users User { get; set; }
        public virtual NewsPost NewsPost{ get; set;}
    }
}