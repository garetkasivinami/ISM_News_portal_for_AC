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
        //================================================================
        public static string FilterToday()
        {
            return $"Date >= CONVERT(DATETIME, '{DateTime.Now.Year}.{DateTime.Now.Month}.{DateTime.Now.Day}')";
        }
        public static string FilterYesterday()
        {
            DateTime yesterday = DateTime.Now.AddDays(-1);
            return $"Date >= CONVERT(DATETIME, '{yesterday.Year}.{yesterday.Month}.{yesterday.Day}') AND Date < CONVERT(DATETIME, '{DateTime.Now.Year}.{DateTime.Now.Month}.{DateTime.Now.Day}')";
        }
        public static string FilterWeek()
        {
            DateTime week = DateTime.Now.AddDays(-7);
            return $"Date >= CONVERT(DATETIME, '{week.Year}.{week.Month}.{week.Day}')";
        }
        public static string FilterAll()
        {
            return "1 = 1";
        }
        public static string GetFilterSqlString(string filter)
        {
            string filterFunc;
            switch (filter)
            {
                case "today":
                    filterFunc = Comment.FilterToday();
                    break;
                case "yesterday":
                    filterFunc = Comment.FilterYesterday();
                    break;
                case "week":
                    filterFunc = Comment.FilterWeek();
                    break;
                default:
                    filterFunc = Comment.FilterAll();
                    break;
            }
            return filterFunc;
        }
        public static string GetSortSqlString(string sortType)
        {
            string sortString;
            switch (sortType)
            {
                case "id":
                    sortString = "@Id";
                    break;
                case "username":
                    sortString = "@UserName";
                    break;
                case "text":
                    sortString = "@text";
                    break;
                case "newsPost":
                    sortString = "@NewsPostId";
                    break;
                default:
                    sortString = "@Date DESC";
                    break;
            }
            return sortString;
        }
        public static string GetSearchSqlString(string searchType)
        {
            string searchString;
            switch (searchType)
            {
                case "text":
                    searchString = "Text LIKE :searchName ";
                    break;
                default:
                    searchString = "UserName LIKE :searchName ";
                    break;
            }
            return searchString;
        }
        public static IQuery GetSqlQuerry(ISession session, string sortType, string filter, string search, string searchString)
        {
            search = search ?? "_";
            return session.CreateQuery("from Comment where " +
                   $"{filter} AND " +
                    searchString +
                   $"ORDER BY {sortType}").
                    SetParameter("searchName", $"%{search}%");
        }
        public static CommentViewModelCollection GenerateCommentViewModelCollection(ToolBarModel model)
        {
            using (ISession session = NHibernateSession.OpenSession())
            {
                string filterFunc = Comment.GetFilterSqlString(model.Filter);
                string sortString = Comment.GetSortSqlString(model.SortType);
                string searchString = Comment.GetSearchSqlString(model.TypeSearch);
                IEnumerable<Comment> comments = Comment.GetSqlQuerry(session, sortString, filterFunc, model.Search, searchString).List<Comment>();

                int commentsCount = comments.Count();

                comments = NewsPostHelperActions.CutIEnumarable(model.Page, Comment.CommentsInOnePage, comments);

                List<CommentViewModel> commentViewModels = new List<CommentViewModel>();
                foreach (Comment comment in comments)
                {
                    commentViewModels.Add(new CommentViewModel(comment));
                }
                model.Pages = NewsPostHelperActions.CalculatePages(commentsCount, Comment.CommentsInOnePage);
                return new CommentViewModelCollection(commentViewModels, model, commentsCount);
            }
        }
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
