namespace ISMNewsPortal.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Comment
    {
        public virtual int Id { get; set; }
        public virtual DateTime Date { get; set; }
        public virtual bool IsEdited { get; set; }
        public virtual string Text { get; set; }
        public virtual NewsPost NewsPost { get; set; }
        public virtual Users User { get; set; }
    }
}
