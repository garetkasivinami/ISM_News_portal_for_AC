using ISMNewsPortal.BLL.Repositories;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using static ISMNewsPortal.BLL.Tools.NewsPostSort;
using ISMNewsPortal.BLL.Models;
using ISMNewsPortal.BLL.BusinessModels;

namespace ISMNewsPortal.DAL.Repositories
{
    public class NewsPostRepository : Repository<NewsPost>, INewsPostRepository
    {
        public int GetCommentsCount(int postId)
        {
            return NHibernateSession.Session.Query<Comment>().Count(u => u.NewsPostId == postId);
        }

        public override IEnumerable<NewsPost> GetWithOptions(object requirements)
        {
            var options = requirements as Options;
            var items = NHibernateSession.Session.Query<NewsPost>();

            if (!options.Admin)
            {
                items = items.Where(u => u.IsVisible == true && u.PublicationDate < DateTime.Now);
            } else {
                var ids = items.Select(u => u.Id);
                items = NHibernateSession.Session.Query<NewsPost>().Where(u => ids.Contains(u.Id));
            }

            if (options.Reversed == true || options.Reversed == null)
                items = SortByReversed(items, options.SortType);
            else
                items = SortBy(items, options.SortType);

            if (options.MinimumDate != null)
                items = items.Where(u => u.PublicationDate >= options.MinimumDate);

            if (options.MaximumDate != null)
                items = items.Where(u => u.PublicationDate < options.MaximumDate);

            if (options.Published != null)
            {
                if (options.Published == true)
                    items = items.Where(u => u.IsVisible == true);
                else
                    items = items.Where(u => u.IsVisible == false);
            }
                

            options.Pages = Helper.CalculatePages(items.Count(), NewsPost.NewsInOnePage);

            items = items.Skip(options.Page * NewsPost.NewsInOnePage).Take(NewsPost.NewsInOnePage);

            var result = items.ToList();

            return result;
        }

        public IEnumerable<NewsPost> GetByAuthorId(int id)
        {
            return NHibernateSession.Session.Query<NewsPost>().Where(u => u.AuthorId == id);
        }

        public IEnumerable<NewsPost> GetByImageId(int id)
        {
            return NHibernateSession.Session.Query<NewsPost>().Where(u => u.ImageId == id);
        }

        public IEnumerable<NewsPost> GetByName(string name)
        {
            return NHibernateSession.Session.Query<NewsPost>().Where(u => u.Name == name);
        }

        public IEnumerable<NewsPost> GetByVisibility(bool visible)
        {
            return NHibernateSession.Session.Query<NewsPost>().Where(u => u.IsVisible == visible);
        }

        public override void Update(NewsPost item)
        {
            var createdNewsPost = NHibernateSession.Session.Get<NewsPost>(item.Id);
            DateTime createdDate = createdNewsPost.CreatedDate;
            item.CreatedDate = createdDate;
            base.Update(item);
        }
    }
}
