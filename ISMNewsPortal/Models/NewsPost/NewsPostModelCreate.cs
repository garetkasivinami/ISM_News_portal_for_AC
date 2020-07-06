using System;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace ISMNewsPortal.Models
{
    public class NewsPostModelCreate
    {
        [Required]
        [MaxLength(128)]
        public string Name { get; set; }
        [Required]
        [MaxLength(10000)]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        [Required]
        public HttpPostedFileBase[] uploadFiles { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime? PublicationDate { get; set; }
        public bool IsVisible { get; set; }
    }
}