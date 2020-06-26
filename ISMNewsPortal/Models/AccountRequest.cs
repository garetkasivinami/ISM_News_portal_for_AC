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
        public const string ErrorMessage = "Це поле необхідно заповнити!";
    }
    public class LoginModel
    {
        [Required(ErrorMessage = HelperActions.ErrorMessage)]
        [RegularExpression(HelperActions.EmailRegex, ErrorMessage = "Введений логін не валідний!")]
        [MaxLength(128)]
        public string Login;
        [Required(ErrorMessage = HelperActions.ErrorMessage)]
        [MinLength(8)]
        [MaxLength(64)]
        public string Password;
    }
    public class RegisterModel
    {
        [Required(ErrorMessage = HelperActions.ErrorMessage)]
        [RegularExpression(HelperActions.EmailRegex, ErrorMessage = "Введений емайл не валідний!")]
        [MinLength(4)]
        [MaxLength(64)]
        [Display(Name = "Ім'я користувача")]
        public string UserName;
        [Required(ErrorMessage = HelperActions.ErrorMessage)]
        [RegularExpression(HelperActions.EmailRegex, ErrorMessage = "Введений емайл не валідний!")]
        [MaxLength(128)]
        [Display(Name = "Ваш емейл")]
        public string Email;
        [Required(ErrorMessage = HelperActions.ErrorMessage)]
        [MinLength(8)]
        [MaxLength(64)]
        [Display(Name = "Ваш пароль")]
        public string Password;
        [Required(ErrorMessage = HelperActions.ErrorMessage)]
        [MinLength(8)]
        [MaxLength(64)]
        [Display(Name = "Повторіть пароль")]
        public string RepeatPassword;
        [Range(1000000, 10000000)]
        [Display(Name = "Номер телефона (не обов'язково)")]
        public int Phone;
        [Range(0,1)]
        public short PhoneCountry;
        [DataType(DataType.MultilineText)]
        public string About;
    }
}