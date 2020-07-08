namespace ISMNewsPortal.Models
{
    using System.ComponentModel.DataAnnotations;

    public class CommentCreateModel
    {
        [Required]
        public int PageId { get; set; }
        [Required]
        [MaxLength(128)]
        [Display(Name = "Username")]
        public string UserName { get; set; }
        [DataType(DataType.MultilineText)]
        [Required]
        [MaxLength(1000)]
        [Display(Name = "Text")]
        public string Text { get; set; }
    }
}
