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

    public partial class NewsPost
    {
        public const int NewsInOnePage = 10;
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
        public virtual DateTime CreatedDate { get; set; }
        public virtual DateTime? EditDate { get; set; }
        public virtual string ImagePath { get; set; }
        public virtual int AuthorId { get; set; }
    }
    public static class NewsPostHelperActions
    {
        public static bool FilterToday(NewsPost newsPost)
        {
            return newsPost.CreatedDate.Day == DateTime.Now.Day;
        }
        public static string FilterToday()
        {
            return $"CreatedDate >= CONVERT(DATETIME, '{DateTime.Now.Year}.{DateTime.Now.Month}.{DateTime.Now.Day}')";
        }
        public static bool FilterYesterday(NewsPost newsPost)
        {
            return newsPost.CreatedDate.Day == DateTime.Now.AddDays(-1).Day;
        }
        public static string FilterYesterday()
        {
            DateTime yesterday = DateTime.Now.AddDays(-1);
            return $"CreatedDate >= CONVERT(DATETIME, '{yesterday.Year}.{yesterday.Month}.{yesterday.Day}') AND CreatedDate < CONVERT(DATETIME, '{DateTime.Now.Year}.{DateTime.Now.Month}.{DateTime.Now.Day}')";
        }
        public static bool FilterWeek(NewsPost newsPost)
        {
            return (DateTime.Now - newsPost.CreatedDate).Days < 7;
        }
        public static string FilterWeek()
        {
            DateTime week = DateTime.Now.AddDays(-7);
            return $"CreatedDate >= CONVERT(DATETIME, '{week.Year}.{week.Month}.{week.Day}')";
        }
        public static bool FilterAll(NewsPost newsPost)
        {
            return true;
        }
        public static string FilterAll()
        {
            return "1 = 1";
        }
        public static NewsPostViewModel GetNewsPostViewModelById(int id)
        {
            using (ISession session = NHibernateSession.OpenSession())
            {
                NewsPost newsPost = session.Get<NewsPost>(id);
                List<CommentViewModel> commentsViewModel = new List<CommentViewModel>();
                IQueryable<Comment> comments = session.Query<Comment>().Where(u => u.NewsPostId == newsPost.Id);
                foreach (Comment comment in comments)
                {
                    commentsViewModel.Add(new CommentViewModel(comment));
                }
                return new NewsPostViewModel(newsPost, commentsViewModel);
            }
        }
        public static IQuery GetSqlQuerry(ISession session, string sortType, string filter, string search)
        {
            //filter = FilterAll();
            search = search ?? "_";
            return session.CreateQuery("from NewsPost where " +
                   $"{filter} AND " +
                    "(Name LIKE :searchName OR " +
                    "Description LIKE :searchName) " +
                   $"ORDER BY {sortType}").
                    SetParameter("searchName", $"%{search}%");
        }
    }
}
