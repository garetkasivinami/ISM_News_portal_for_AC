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
        [Display(Name = "Ваш логін")]
        public string Login { get; set; }
        [Required(ErrorMessage = HelperActions.ErrorMessage)]
        [MinLength(8)]
        [MaxLength(64)]
        [Display(Name = "Ваш пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
    public class RegisterModel
    {
        [Required(ErrorMessage = HelperActions.ErrorMessage)]
        [MinLength(4)]
        [MaxLength(64)]
        [Display(Name = "Ім'я користувача")]
        public string UserName { get; set; }
        [Required(ErrorMessage = HelperActions.ErrorMessage)]
        [RegularExpression(HelperActions.EmailRegex, ErrorMessage = "Введений емайл не валідний!")]
        [MaxLength(128)]
        [Display(Name = "Ваш емейл")]
        public string Email { get; set; }
        [Required(ErrorMessage = HelperActions.ErrorMessage)]
        [MinLength(8)]
        [MaxLength(64)]
        [Display(Name = "Ваш пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = HelperActions.ErrorMessage)]
        [MinLength(8)]
        [MaxLength(64)]
        [Display(Name = "Повторіть пароль")]
        [DataType(DataType.Password)]
        public string RepeatPassword { get; set; }
        [DataType(DataType.PhoneNumber)]
        [Range(1000000, 10000000, ErrorMessage = "Введений номер телефона не коректний!")]
        [Display(Name = "Номер телефона (не обов'язково)")]
        public int? Phone { get; set; }
        [Range(0,1)]
        public short? PhoneCountry { get; set; }
        [DataType(DataType.MultilineText)]
        [Display(Name = "Про мене (не обов'язково)")]
        public string About { get; set; }
    }
}