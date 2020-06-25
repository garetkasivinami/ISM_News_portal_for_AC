namespace ISMNewsPortal.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Comment
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public DateTime Date { get; set; }

        public bool IsEdited { get; set; }

        [Required]
        public string Text { get; set; }

        public int NewsPostId { get; set; }

        public virtual NewsPost NewsPost { get; set; }

        public virtual User User { get; set; }
    }
}
