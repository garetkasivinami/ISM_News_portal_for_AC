namespace ISMNewsPortal.Models
{
    using System.ComponentModel.DataAnnotations;

    public class CommentCreateModel
    {
        [Required]
        public int PageId { get; set; }
        [Required]
        [MaxLength(128)]
        public string UserName { get; set; }
        [DataType(DataType.MultilineText)]
        [Required]
        [MaxLength(1000)]
        public string Text { get; set; }
    }
}
