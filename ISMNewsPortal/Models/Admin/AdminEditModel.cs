using ISMNewsPortal.BLL.DTO;
using ISMNewsPortal.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ISMNewsPortal.Models
{
    public class AdminEditModel
    {
        public AdminEditModel()
        {

        }
        public AdminEditModel(Admin admin)
        {
            Id = admin.Id;
            Login = admin.Login;
            Email = admin.Email;
            Roles = AdminHelper.RoleCutter(admin.Roles).ToArray();
        }
        public int Id { get; set; }
        public string Login { get; set; }
        [MaxLength(512)]
        [Display(Name = "Email", ResourceType = typeof(Language.Language))]
        [RegularExpression(HelperActions.EmailRegex)]
        public string Email { get; set; }
        public string[] Roles { get; set; }
    }
}