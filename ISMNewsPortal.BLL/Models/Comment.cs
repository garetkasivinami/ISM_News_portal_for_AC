using System;

namespace ISMNewsPortal.BLL.Models
{
    public partial class Comment : Model
    {
        public const int CommentsInOnePage = 10;
        public string UserName { get; set; }
        public DateTime Date { get; set; }
        public string Text { get; set; }
        public int NewsPostId { get; set; }
    }
}
