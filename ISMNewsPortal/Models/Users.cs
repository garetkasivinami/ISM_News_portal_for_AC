namespace ISMNewsPortal.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

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
        public virtual IList<NewsPost> NewsPosts { get; set; }
    }
}
