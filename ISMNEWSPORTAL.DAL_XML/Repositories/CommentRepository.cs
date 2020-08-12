using ISMNewsPortal.BLL.BusinessModels;
using ISMNewsPortal.BLL.Models;
using ISMNewsPortal.BLL.Repositories;
using System.Collections.Generic;
using System.Linq;
using static ISMNewsPortal.BLL.Services.CommentService;

namespace ISMNewsPortal.DAL_XML.Repositories
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

        public override IEnumerable<Comment> GetWithOptions(object requirements)
        {
            var options = requirements as OptionsCollectionById;
            IEnumerable<Comment> items = base.GetWithOptions(options).Where(u => u.NewsPostId == options.TargetId);
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

            options.Pages = CalculatePages(items.Count(), Comment.CommentsInOnePage);

            options.CommentsCount = items.Count();

            items = items.Skip(options.Page * Comment.CommentsInOnePage).Take(Comment.CommentsInOnePage);

            var result = items.ToList();

            return result;
        }
    }
}
