using ISMNewsPortal.BLL.BusinessModels;
using ISMNewsPortal.BLL.Models;
using ISMNewsPortal.BLL.Repositories;
using ISMNewsPortal.Lucene;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISMNEWSPORTAL.DAL_XML.Repositories
{
    public class CommentRepository : Repository<Comment>, ICommentRepository
    {
        public CommentRepository(XMLContex contex) : base(contex)
        {

        }

        public void DeleteCommentsByPostId(int postId)
        {
            var comments = GetAll().Where(u => u.NewsPostId == postId);
            foreach (Comment comment in comments)
                Delete(comment.Id);
        }

        public IEnumerable<Comment> GetByPostId(int id)
        {
            return GetAll().Where(u => u.NewsPostId == id);
        }

        public IEnumerable<Comment> GetByUserName(string userName)
        {
            return GetAll().Where(u => u.UserName == userName);
        }

        public IEnumerable<Comment> GetByUserNameAndPostId(string userName, int postId)
        {
            return GetAll().Where(u => u.UserName == userName && u.NewsPostId == postId);
        }

        public int GetCountByPostId(int id)
        {
            return GetAll().Where(u => u.NewsPostId == id).Count();
        }

        public static int CalculatePages(int count, int countInOnePage)
        {
            int pages = count / countInOnePage;
            if (count % countInOnePage != 0)
            {
                pages++;
            }
            return pages;
        }

        public static IEnumerable<Comment> SortBy(IEnumerable<Comment> items, string sortType, bool reversed)
        {
            sortType = sortType?.ToLower();
            switch (sortType)
            {
                case "id":
                    items = items.OrderBy(u => u.Id);
                    break;
                case "username":
                    items = items.OrderBy(u => u.UserName);
                    break;
                case "commenttext":
                    items = items.OrderBy(u => u.Text);
                    break;
                default:
                    items = items.OrderBy(u => u.Date);
                    break;
            }
            if (reversed)
                items = items.Reverse();
            return items;
        }

        public override IEnumerable<Comment> GetWithOptions(object requirements)
        {
            var options = requirements as OptionsCollectionById;
            IEnumerable<Comment> items = base.GetWithOptions(options).Where(u => u.NewsPostId == options.TargetId);
            if (!string.IsNullOrEmpty(options.Search))
            {
                items = items.Where(u => (u.Text.Contains(options.Search) || u.UserName.Contains(options.Search)));
            }

            items = SortBy(items, options.SortType, options.Reversed ?? true);

            if (options.MinimumDate != null)
                items = items.Where(u => u.Date >= options.MinimumDate);

            if (options.MaximumDate != null)
                items = items.Where(u => u.Date < options.MaximumDate);

            options.Pages = CalculatePages(items.Count(), Comment.CommentsInOnePage);

            items = items.Skip(options.Page * Comment.CommentsInOnePage).Take(Comment.CommentsInOnePage);

            var result = items.ToList();

            return result;
        }
    }
}
