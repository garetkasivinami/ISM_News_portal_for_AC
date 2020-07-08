namespace ISMNewsPortal.Models
{
    using NHibernate;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Data.SqlTypes;
    using System.Linq;
    using System.Web;

    public partial class NewsPost
    {
        public const int NewsInOnePage = 10;
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
        public virtual DateTime CreatedDate { get; set; }
        public virtual DateTime? EditDate { get; set; }
        public virtual int AuthorId { get; set; }
        public virtual bool IsVisible { get; set; }
        public virtual DateTime PublicationDate { get; set; }
        public virtual int ImageId { get; set; }
        //===============================================================================
        public NewsPost()
        {

        }
        public NewsPost(NewsPostCreateModel model)
        {
            Name = model.Name;
            Description = model.Description;
            CreatedDate = DateTime.Now;
            EditDate = null;
            ImageId = model.ImageId;
            AuthorId = model.AuthorId;
            IsVisible = model.IsVisible;
            PublicationDate = model.PublicationDate ?? DateTime.Now;
        }
        public NewsPost(NewsPostEditModel model)
        {
            Id = model.Id;
            Name = model.Name;
            Description = model.Description;
            CreatedDate = model.CreatedDate;
            EditDate = DateTime.Now;
            ImageId = model.ImageId;
            AuthorId = model.AuthorId;
            IsVisible = model.IsVisible;
            PublicationDate = model.PublicationDate ?? DateTime.Now;
        }
        public static void Update(NewsPostEditModel model)
        {
            using (ISession session = NHibernateSession.OpenSession())
            {
                NewsPost newsPost = new NewsPost(model);
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Update(newsPost);
                    transaction.Commit();
                }
            }
        }
        public static void RemoveNewsPost(int id)
        {
            using (ISession session = NHibernateSession.OpenSession())
            {
                NewsPost newsPost = session.Get<NewsPost>(id);
                IEnumerable<Comment> comments = session.Query<Comment>().Where(u => u.NewsPostId == id);
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Delete(newsPost);
                    foreach(Comment comment in comments)
                    {
                        session.Delete(comment);
                    }
                    transaction.Commit();
                }
                FileModel.Delete(newsPost.ImageId);
            }
        }
        public static void AddNewsPost(NewsPostCreateModel model)
        {
            using (ISession session = NHibernateSession.OpenSession())
            {
                NewsPost newsPost = new NewsPost(model);
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Save(newsPost);
                    transaction.Commit();
                }
            }
        }
    }
}
