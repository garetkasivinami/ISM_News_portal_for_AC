namespace ISMNewsPortal.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class User
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User()
        {
            Comments = new HashSet<Comment>();
            NewsPosts = new HashSet<NewsPost>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(128)]
        public string Login { get; set; }

        [Required]
        [StringLength(128)]
        public string Password { get; set; }

        [Required]
        [StringLength(64)]
        public string UserName { get; set; }

        public bool IsActivated { get; set; }

        public int? Phone { get; set; }

        public short? PhoneCountry { get; set; }

        [StringLength(1024)]
        public string About { get; set; }

        public DateTime RegistrationDate { get; set; }

        public bool IsAdmin { get; set; }

        public int CommentsCount { get; set; }

        public bool IsBanned { get; set; }

        public byte WarningsCount { get; set; }

        [StringLength(256)]
        public string AvatarPath { get; set; }

        public bool HideLogin { get; set; }

        public bool HidePhone { get; set; }

        public bool HideCommentsCount { get; set; }

        public bool HideRegistrationDate { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Comment> Comments { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NewsPost> NewsPosts { get; set; }
    }
}
