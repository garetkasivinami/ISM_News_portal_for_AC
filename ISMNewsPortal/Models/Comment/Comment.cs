namespace ISMNewsPortal.Models
{
    using NHibernate;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Linq;

    public partial class Comment
    {
        public const int CommentsInOnePage = 10;
        public virtual int Id { get; set; }
        public virtual string UserName { get; set; }
        public virtual DateTime Date { get; set; }
        public virtual string Text { get; set; }
        public virtual int NewsPostId { get; set; }

        public Comment()
        {

        }

        public Comment(CommentCreateModel createModel)
        {
            UserName = createModel.UserName;
            Date = DateTime.Now;
            Text = createModel.Text;
            NewsPostId = createModel.PageId;
        }
    }
}
