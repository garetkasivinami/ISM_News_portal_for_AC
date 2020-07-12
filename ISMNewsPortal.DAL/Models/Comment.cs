using System;

namespace ISMNewsPortal.DAL.Models
{
    public partial class Comment
    {
        public const int CommentsInOnePage = 10;
        public virtual int Id { get; set; }
        public virtual string UserName { get; set; }
        public virtual DateTime Date { get; set; }
        public virtual string Text { get; set; }
        public virtual int NewsPostId { get; set; }
    }
}
