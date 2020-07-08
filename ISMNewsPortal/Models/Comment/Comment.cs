namespace ISMNewsPortal.Models
{
    using NHibernate;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Linq;

    public partial class Comment
    {
        public const int CommentsInOnePage = 10;
        public virtual int Id { get; set; }
        public virtual string UserName { get; set; }
        public virtual DateTime Date { get; set; }
        public virtual string Text { get; set; }
        public virtual int NewsPostId { get; set; }
        //================================================
        public static void AddNewComment(CommentCreateModel model)
        {
            using (ISession session = NHibernateSession.OpenSession())
            {
                Comment comment = new Comment();
                comment.Date = DateTime.Now;
                comment.NewsPostId = model.PageId;
                comment.Text = model.Text;
                comment.UserName = model.UserName;
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Save(comment);
                    transaction.Commit();
                }
            }
        }
        public static void RemoveComment(int id)
        {
            using (ISession session = NHibernateSession.OpenSession())
            {
                Comment comment = session.Get<Comment>(id);
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Delete(comment);
                    transaction.Commit();
                }
            }
        }
    }
}
