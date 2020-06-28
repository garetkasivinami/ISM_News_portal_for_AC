namespace ISMNewsPortal.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class NewsPost
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string Descrition { get; set; }
        public virtual DateTime CreatedDate { get; set; }
        public virtual int CommentsCount { get; set; }
        public virtual DateTime? EditDate { get; set; }
        public virtual string ImagePath { get; set; }
        public virtual bool ForRegistered { get; set; }
        public virtual IList<Comment> Comments { get; set; }
        public virtual Admin Author { get; set; }
    }
}
