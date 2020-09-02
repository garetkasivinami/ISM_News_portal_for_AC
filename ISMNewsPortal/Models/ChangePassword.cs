using System.ComponentModel.DataAnnotations;

namespace ISMNewsPortal.Models
{
    public class ChangePassword
    {
        [Required]
        [MinLength(Security.MinPasswordLength)]
        [MaxLength(Security.MaxPasswordLength)]
        [Display(Name = "OldPassword", ResourceType = typeof(Language.Language))]
        [DataType(DataType.Password)]
        public string LastPassword { get; set; }

        [Required]
        [MinLength(Security.MinPasswordLength)]
        [MaxLength(Security.MaxPasswordLength)]
        [Display(Name = "NewPassword", ResourceType = typeof(Language.Language))]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [MinLength(Security.MinPasswordLength)]
        [MaxLength(Security.MaxPasswordLength)]
        [Display(Name = "RepPassword", ResourceType = typeof(Language.Language))]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string RepeatPassword { get; set; }
    }
}