using ISMNewsPortal.DAL.Interfaces;
using ISMNewsPortal.DAL.Models;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ISMNewsPortal.DAL.ToolsLogic.CommentToolsLogic;

namespace ISMNewsPortal.DAL.Repositories
{
    public class CommentRepository : IRepository<Comment>
    {
        private ISession session;

        public CommentRepository(ISession session)
        {
            this.session = session;
        }

        public int Create(Comment item)
        {
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Save(item);
                transaction.Commit();
                return item.Id;
            }
        }

        public void Delete(int id)
        {
            var item = session.Get<Comment>(id);
            if (item == null)
                return;
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Delete(item);
                transaction.Commit();
            }
        }

        public IEnumerable<Comment> Find(Func<Comment, bool> predicate)
        {
            return session.Query<Comment>().Where(predicate);
        }

        public Comment Get(int id)
        {
            return session.Get<Comment>(id);
        }

        public IEnumerable<Comment> GetAll()
        {
            return session.Query<Comment>();
        }

        public int Count()
        {
            return session.Query<Comment>().Count();
        }

        public int Count(Func<Comment, bool> predicate)
        {
            return session.Query<Comment>().Count(predicate);
        }

        public void Update(Comment item)
        {
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Update(item);
                transaction.Commit();
            }
        }

        public IEnumerable<Comment> GetAllWithTools(ToolBarModel model)
        {
            string filterFunc = GetFilterSqlString(model.Filter);
            string sortString = GetSortSqlString(model.SortType, model.Reversed ?? false);
            string searchString = GetSearchSqlString(model.TypeSearch);
            IList<Comment> comments = GetSqlQuerry(session, sortString, filterFunc, model.Search, searchString).List<Comment>();

            model.Pages = Helper.CalculatePages(comments.Count, Comment.CommentsInOnePage);

            return Helper.CutIEnumarable(model.Page, Comment.CommentsInOnePage, comments);
        }
    }
}
