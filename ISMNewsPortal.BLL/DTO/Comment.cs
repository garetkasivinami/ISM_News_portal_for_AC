using System;

namespace ISMNewsPortal.BLL.DTO
{
    public partial class Comment
    {
        public const int CommentsInOnePage = 10;
        public int Id { get; set; }
        public string UserName { get; set; }
        public DateTime Date { get; set; }
        public string Text { get; set; }
        public int NewsPostId { get; set; }
    }
}
