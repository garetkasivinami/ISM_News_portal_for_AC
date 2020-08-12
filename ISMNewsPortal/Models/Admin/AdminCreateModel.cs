using System.ComponentModel.DataAnnotations;

namespace ISMNewsPortal.Models
{
    public class AdminCreateModel
    {
        [Required]
        [MinLength(4)]
        [MaxLength(128)]
        [Display(Name = "Login", ResourceType = typeof(Language.Language))]
        public string Login { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [MinLength(8)]
        [MaxLength(64)]
        [Display(Name = "Password", ResourceType = typeof(Language.Language))]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [MinLength(8)]
        [MaxLength(64)]
        [Compare("Password")]
        [Display(Name = "RepPassword", ResourceType = typeof(Language.Language))]
        public string RepeatPassword { get; set; }
        /*Регулярка*/
        [MaxLength(512)]
        [Display(Name = "Email", ResourceType = typeof(Language.Language))]
        [RegularExpression(HelperActions.EmailRegex)]
        public string Email { get; set; }
    }
}