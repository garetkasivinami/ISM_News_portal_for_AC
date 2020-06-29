using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ISMNewsPortal.Models
{
    public static class HelperActions
    {
        public const string EmailRegex = @"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$";
        public const string ErrorMessage = "This field is required!";
    }
    public class LoginModel
    {
        [Required(ErrorMessage = HelperActions.ErrorMessage)]
        [RegularExpression(HelperActions.EmailRegex, ErrorMessage = "The login you entered was not valid!")]
        [MaxLength(128)]
        [Display(Name = "Login")]
        public string Login { get; set; }
        [Required(ErrorMessage = HelperActions.ErrorMessage)]
        [MinLength(8)]
        [MaxLength(64)]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
    public class RegisterModel
    {
        [Required(ErrorMessage = HelperActions.ErrorMessage)]
        [MinLength(4)]
        [MaxLength(64)]
        [Display(Name = "Username")]
        public string UserName { get; set; }
        [Required(ErrorMessage = HelperActions.ErrorMessage)]
        [RegularExpression(HelperActions.EmailRegex, ErrorMessage = "The email you entered was not valid!")]
        [MaxLength(128)]
        [Display(Name = "Email adress")]
        public string Email { get; set; }
        [Required(ErrorMessage = HelperActions.ErrorMessage)]
        [MinLength(8)]
        [MaxLength(64)]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = HelperActions.ErrorMessage)]
        [MinLength(8)]
        [MaxLength(64)]
        [Display(Name = "Confirm password")]
        [DataType(DataType.Password)]
        public string RepeatPassword { get; set; }
        [DataType(DataType.PhoneNumber)]
        [Range(1000000, 10000000, ErrorMessage = "The phone number you entered was not valid!")]
        [Display(Name = "Phone number (optional)")]
        public int? Phone { get; set; }
        [Range(0,1)]
        public short? PhoneCountry { get; set; }
        [DataType(DataType.MultilineText)]
        [Display(Name = "About me (optional)")]
        public string About { get; set; }
    }
}