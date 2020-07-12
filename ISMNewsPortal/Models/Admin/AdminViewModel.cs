using System.Collections.Generic;

namespace ISMNewsPortal.Models
{
    public class AdminViewModel
    {
        public AdminViewModel(Admin admin)
        {
            Id = admin.Id;
            Login = admin.Login;
            Email = admin.Email;
            Roles = AdminHelper.RoleCutter(admin.Roles);
        }
        public int Id { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
}