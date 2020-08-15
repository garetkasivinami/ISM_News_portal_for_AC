﻿using ISMNewsPortal.BLL.BusinessModels;
using ISMNewsPortal.BLL.Models;
using ISMNewsPortal.BLL.Repositories;
using NHibernate;
using System.Collections.Generic;
using System.Linq;
using static ISMNewsPortal.BLL.Services.CommentService;

namespace ISMNewsPortal.DAL.Repositories
{
    public class CommentRepository : Repository<Comment>, ICommentRepository
    {

        public CommentRepository(HibernateUnitOfWork hibernateUnitOfWork) : base(hibernateUnitOfWork)
        {

        }

        public int GetCountByPostId(int id)
        {
            return hibernateUnitOfWork.Session.Query<Comment>().Count(u => u.NewsPostId == id);
        }

        public void DeleteCommentsByPostId(int postId)
        {
            hibernateUnitOfWork.BeginTransaction();
            var comments = hibernateUnitOfWork.Session.Query<Comment>().Where(u => u.NewsPostId == postId);
            foreach (Comment comment in comments)
                hibernateUnitOfWork.Session.Delete(comment);
        }

        public IEnumerable<Comment> GetByPostId(int id)
        {
            return hibernateUnitOfWork.Session.Query<Comment>().Where(u => u.NewsPostId == id);
        }

        public IEnumerable<Comment> GetByUserName(string userName)
        {
            return hibernateUnitOfWork.Session.Query<Comment>().Where(u => u.UserName == userName);
        }

        public IEnumerable<Comment> GetByUserNameAndPostId(string userName, int postId)
        {
            return hibernateUnitOfWork.Session.Query<Comment>().Where(u => u.NewsPostId == postId && u.UserName == userName);
        }

        public override IEnumerable<Comment> GetWithOptions(object requirements)
        {
            var options = requirements as OptionsCollectionById;
            IEnumerable<Comment> items = hibernateUnitOfWork.Session.Query<Comment>().Where(u => u.NewsPostId == options.TargetId);

            if (!string.IsNullOrEmpty(options.Search))
            {
                items = items.Where(u => (u.Text.Contains(options.Search) || u.UserName.Contains(options.Search)));
            }

            if (options.Reversed == true || options.Reversed == null)
                items = SortByReversed(items, options.SortType);
            else
                items = SortBy(items, options.SortType);

            if (options.MinimumDate != null)
                items = items.Where(u => u.Date >= options.MinimumDate);

            if (options.MaximumDate != null)
                items = items.Where(u => u.Date < options.MaximumDate);

            options.Pages = Helper.CalculatePages(items.Count(), Comment.CommentsInOnePage);

            options.CommentsCount = items.Count();

            items = items.Skip(options.Page * Comment.CommentsInOnePage).Take(Comment.CommentsInOnePage);

            var result = items.ToList();

            return result;
        }

    }
}
