namespace ISMNewsPortal.Models
{
    using System.ComponentModel.DataAnnotations;

    public class CommentCreateModel
    {
        [Required]
        public int PageId { get; set; }
        [Required]
        [MaxLength(100)]
        [Display(Name = "Username", ResourceType = typeof(Language.Language))]
        public string UserName { get; set; }
        [DataType(DataType.MultilineText)]
        [Required]
        [Display(Name = "CommentText", ResourceType = typeof(Language.Language))]
        public string Text { get; set; }
    }
}
