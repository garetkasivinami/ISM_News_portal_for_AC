namespace ISMNewsPortal.Models
{
    using Microsoft.Ajax.Utilities;
    using NHibernate;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Configuration;
    using System.Data.Entity.Spatial;
    using System.Linq;
    using System.Security.Principal;
    using System.Web;

    public partial class Users
    {
        public virtual int Id { get; set; }
        public virtual string Login { get; set; }
        public virtual string Password { get; set; }
        public virtual string Salt { get; set; }
        public virtual string UserName { get; set; }
        public virtual bool IsActivated { get; set; }
        public virtual int? Phone { get; set; }
        public virtual short? PhoneCountry { get; set; }
        public virtual string About { get; set; }
        public virtual DateTime RegistrationDate { get; set; }
        public virtual int CommentsCount { get; set; }
        public virtual int LikesCount { get; set; }
        public virtual bool IsBanned { get; set; }
        public virtual byte WarningsCount { get; set; }
        public virtual string AvatarPath { get; set; }
        public virtual bool HideLogin { get; set; }
        public virtual bool HidePhone { get; set; }
        public virtual bool HideCommentsCount { get; set; }
        public virtual bool HideRegistrationDate { get; set; }
        public virtual IList<Comment> Comments { get; set; }
        public virtual IList<UserLike> Likes { get; set; }
        public virtual IList<NewsPost> NewsPosts { get; set; }
        public static void ChangePassword(Users user, string password)
        {
            ChangePassword(user, password, Security.RandomString(Security.PasswordUserLength));
        }
        public static void ChangePassword(Users user, string password, string salt)
        {
            user.Salt = salt;
            user.Password = Security.SHA512(password, salt);
        }
        public static bool ComparePasswords(Users user, string password)
        {
            return user.Password == Security.SHA512(password, user.Salt);
        }
        public static Users GetUserByLogin(string login)
        {
            Users user;
            using (ISession session = NHibernateSession.OpenSession())
            {
                user = GetUserByLogin(login, session);
            }
            return user;
        }
        public static Users GetUserByLogin(string login, ISession session)
        {
            return session.Query<Users>().FirstOrDefault(u => u.Login == login);
        }
    }
}
