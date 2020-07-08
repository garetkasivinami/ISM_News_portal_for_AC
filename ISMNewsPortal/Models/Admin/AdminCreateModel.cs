﻿using System;
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
        [Display(Name = "Login")]
        public string Login { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [MinLength(8)]
        [MaxLength(128)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [MinLength(8)]
        [MaxLength(128)]
        [Compare("Password")]
        [Display(Name = "Repeat password")]
        public string RepeatPassword { get; set; }
        /*Регулярка*/
        [MaxLength(512)]
        [Display(Name = "Email")]
        [RegularExpression(HelperActions.EmailRegex)]
        public string Email { get; set; }
    }
}