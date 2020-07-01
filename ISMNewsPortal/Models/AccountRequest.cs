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
        public string Login { get; set; }
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
        [Range(0, 1)]
        public short? PhoneCountry { get; set; }
        [DataType(DataType.MultilineText)]
        [Display(Name = "About me (optional)")]
        public string About { get; set; }
    }
    public class UserSafeModel
    {
        public UserSafeModel(Users user)
        {
            Id = user.Id;
            Login = user.Login;
            UserName = user.UserName;
            Phone = user.Phone;
            PhoneCountry = user.PhoneCountry;
            About = user.About;
            RegistrationDate = user.RegistrationDate;
            CommentsCount = user.CommentsCount;
            LikesCount = user.LikesCount;
            IsBanned = user.IsBanned;
            WarningsCount = user.WarningsCount;
            AvatarPath = user.AvatarPath;
            HideCommentsCount = user.HideCommentsCount;
            HideLogin = user.HideLogin;
            HidePhone = user.HidePhone;
            HideRegistrationDate = user.HideRegistrationDate;
        }
        public int Id { get; set; }
        public string Login { get; set; }
        public string UserName { get; set; }
        public int? Phone { get; set; }
        public short? PhoneCountry { get; set; }
        public string About { get; set; }
        public DateTime RegistrationDate { get; set; }
        public int CommentsCount { get; set; }
        public int LikesCount { get; set; }
        public bool IsBanned { get; set; }
        public byte WarningsCount { get; set; }
        public string AvatarPath { get; set; }
        public bool HideLogin { get; set; }
        public bool HidePhone { get; set; }
        public bool HideCommentsCount { get; set; }
        public bool HideRegistrationDate { get; set; }
        public void OpenHidden()
        {
            HideCommentsCount = false;
            HideLogin = false;
            HidePhone = false;
            HideRegistrationDate = false;
        }
    }
}