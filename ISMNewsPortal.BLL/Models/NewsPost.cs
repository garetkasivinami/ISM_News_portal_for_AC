using System;

namespace ISMNewsPortal.BLL.Models
{
    public partial class NewsPost : Model
    {
        public const int NewsInOnePage = 10;
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? EditDate { get; set; }
        public int AuthorId { get; set; }
        public bool IsVisible { get; set; }
        public DateTime PublicationDate { get; set; }
        public int ImageId { get; set; }
    }
}
