using System;
using System.Collections.Generic;
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
            Roles = Admin.RoleCutter(admin.Roles).ToArray();
        }
        public int Id { get; set; }
        public string Login { get; set; }
        /*Регулярка*/
        public string Email { get; set; }
        public string[] Roles { get; set; }
    }
}