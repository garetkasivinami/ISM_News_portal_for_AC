using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Web;

namespace ISMNewsPortal.BLL.DTO
{
    public partial class NewsPostDTO
    {
        public const int NewsInOnePage = 10;
        public int Id { get; set; }
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
