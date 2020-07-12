namespace ISMNewsPortal.Models
{
    using System.ComponentModel.DataAnnotations;

    public class CommentCreateModel
    {
        [Required]
        public int PageId { get; set; }
        [Required]
        [MaxLength(100)]
        [Display(Name = "Username")]
        public string UserName { get; set; }
        [DataType(DataType.MultilineText)]
        [Required]
        [Display(Name = "Text")]
        public string Text { get; set; }
    }
}
