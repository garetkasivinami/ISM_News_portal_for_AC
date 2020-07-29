using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ISMNewsPortal.Models
{
    public class ChangePassword
    {
        [MinLength(8)]
        [MaxLength(128)]
        [Display(Name = "OldPassword", ResourceType = typeof(Language.Language))]
        [DataType(DataType.Password)]
        public string LastPassword { get; set; }
        [MinLength(8)]
        [MaxLength(128)]
        [Display(Name = "NewPassword", ResourceType = typeof(Language.Language))]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [MinLength(8)]
        [MaxLength(128)]
        [Display(Name = "RepPassword", ResourceType = typeof(Language.Language))]
        [DataType(DataType.Password)]
        public string RepeatPassword { get; set; }
    }
}