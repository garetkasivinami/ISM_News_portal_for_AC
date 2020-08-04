using ISMNewsPortal.BLL.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISMNewsPortal.Models
{
    public class User : Admin, IUser<int>
    {
        public string UserName { get => Login; set => Login = value; }

        public User()
        {

        }
        public User(Admin admin)
        {
            Id = admin.Id;
            Login = admin.Login;
            Password = admin.Password;
            Salt = admin.Salt;
            Email = admin.Email;
            Roles = admin.Roles;
        }
    }
}