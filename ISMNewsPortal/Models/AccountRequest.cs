using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace ISMNewsPortal.Models
{
    public static class HelperActions
    {
        public const string EmailRegex = @"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$";
        public const string ErrorMessage = "This field is required!";
        public const string XssInjectRegex = @"((\%3C)|<)((\%2F)|\/)*[a-z0-9\%]+((\%3E)|>)";
        public const string XssIndectDetectedError = "XSS atack detected!";

        public static bool XSSAtackCheker(params string[] lines)
        {
            Regex regex = new Regex(XssInjectRegex);
            foreach(string line in lines)
            {
                if (regex.IsMatch(line))
                {
                    return true;
                }
            }
            return false;
        }
    }
    public class LoginModel
    {
        [Required(ErrorMessage = HelperActions.ErrorMessage)]
        [MaxLength(128)]
        [Display(Name = "Login", ResourceType = typeof(Language.Language))]
        public string Login { get; set; }
        [Required(ErrorMessage = HelperActions.ErrorMessage)]
        [MinLength(8)]
        [MaxLength(128)]
        [Display(Name = "Password", ResourceType = typeof(Language.Language))]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}