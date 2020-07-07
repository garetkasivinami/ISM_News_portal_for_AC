using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ISMNewsPortal.Models
{
    public class AdminCreateModel
    {
        [Required]
        [MinLength(4)]
        [MaxLength(128)]
        public string Login { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [MinLength(8)]
        [MaxLength(128)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [MinLength(8)]
        [MaxLength(128)]
        [Compare("Password")]
        public string RepeatPassword { get; set; }
        /*Регулярка*/
        public string Email { get; set; }
    }
}