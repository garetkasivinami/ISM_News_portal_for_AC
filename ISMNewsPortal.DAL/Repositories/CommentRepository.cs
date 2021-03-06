﻿using ISMNewsPortal.BLL.BusinessModels;
using ISMNewsPortal.BLL.Models;
using ISMNewsPortal.BLL.Repositories;
using NHibernate;
using System.Collections.Generic;
using System.Linq;
using static ISMNewsPortal.BLL.Helpers.DALHelper;
using static ISMNewsPortal.BLL.Tools.CommentsSort;

namespace ISMNewsPortal.DAL.Repositories
{
    public class CommentRepository : Repository<Comment>, ICommentRepository
    {

        public CommentRepository() : base()
        {

        }

        public int GetCountByPostId(int id)
        {
            return NHibernateSession.Session.Query<Comment>().Count(u => u.NewsPostId == id);
        }

        public void DeleteCommentsByPostId(int postId)
        {
            hibernateUnitOfWork.BeginTransaction();
            ISession session = NHibernateSession.Session;
            var comments = session.Query<Comment>().Where(u => u.NewsPostId == postId);
            foreach (Comment comment in comments)
                session.Delete(comment);
        }

        public IEnumerable<Comment> GetByPostId(int id)
        {
            return NHibernateSession.Session.Query<Comment>().Where(u => u.NewsPostId == id).ToList();
        }

        public IEnumerable<Comment> GetByUserName(string userName)
        {
            return NHibernateSession.Session.Query<Comment>().Where(u => u.UserName == userName).ToList();
        }

        public IEnumerable<Comment> GetByUserNameAndPostId(string userName, int postId)
        {
            return NHibernateSession.Session.Query<Comment>().Where(u => u.NewsPostId == postId && u.UserName == userName).ToList();
        }

        public override IEnumerable<Comment> GetWithOptions(object requirements)
        {
            var options = requirements as OptionsCollectionById;
            IQueryable<Comment> items = NHibernateSession.Session.Query<Comment>().Where(u => u.NewsPostId == options.TargetId);

            if (!string.IsNullOrEmpty(options.Search))
            {
                items = items.Where(u => (u.Text.Contains(options.Search) || u.UserName.Contains(options.Search)));
            }

            if (options.Reversed == true || options.Reversed == null)
                items = SortByReversed(items, options.SortType);
            else
                items = SortBy(items, options.SortType);

            if (options.DateRange.StartDate != null)
                items = items.Where(u => u.Date >= options.DateRange.StartDate);

            if (options.DateRange.EndDate != null)
                items = items.Where(u => u.Date < options.DateRange.EndDate);

            options.Pages = CalculatePages(items.Count(), Comment.CommentsInOnePage);

            options.ItemsCount = items.Count();

            items = items.Skip(options.Page * Comment.CommentsInOnePage).Take(Comment.CommentsInOnePage);

            return items.ToList();
        }

    }
}
