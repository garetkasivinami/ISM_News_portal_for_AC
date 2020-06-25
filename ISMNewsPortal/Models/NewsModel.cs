namespace ISMNewsPortal.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class NewsModel : DbContext
    {
        public NewsModel()
            : base("name=NewsModel")
        {
        }

        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<NewsPost> NewsPosts { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NewsPost>()
                .HasMany(e => e.Comments)
                .WithRequired(e => e.NewsPost)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Comments)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.NewsPosts)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.AuthorId)
                .WillCascadeOnDelete(false);
        }
    }
}
