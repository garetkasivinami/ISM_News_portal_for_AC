namespace ISMNewsPortal.BLL.Models
{
    public class Admin : Model
    {
        public string Login { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public string Roles { get; set; }
    }
}