using ISMNewsPortal.BLL.BusinessModels;
using ISMNewsPortal.BLL.Models;
using System.Collections.Generic;
using System.Linq;
using ISMNewsPortal.BLL.Exceptions;
using static ISMNewsPortal.BLL.SessionManager;
using System.ComponentModel;

namespace ISMNewsPortal.BLL.Services
{
    public class CommentService
    {
        private const string CommentCache = "comment";
        private const string CommentsByPostIdCache = "comments-postId";
        private const string CommentsWithToolsCache = "tools";

        public Comment GetComment(int id)
        {
            var comment = CommentRepository.Get(id);
            if (comment == null)
                throw new CommentNullException();
            return comment;
        }

        public IEnumerable<Comment> GetCommentsByPostId(int id)
        {
            var comments = CacheRepository.GetItems<Comment>($"{CommentsByPostIdCache}-{id}");
            if (comments != null)
                return comments;

            comments = CommentRepository.GetByPostId(id).Reverse();
            CacheRepository.Add(comments, $"{CommentsByPostIdCache}-{id}");
            return comments;
        }

        public int CreateComment(Comment comment)
        {
            int id = CommentRepository.Create(comment);
            UnitOfWork.Save();
            CacheRepository.DeleteByPartOfTheKey($"{CommentsByPostIdCache}-{comment.NewsPostId}");
            return id;
        }

        public IEnumerable<Comment> GetCommentsWithTools(OptionsCollectionById options)
        {
            var comments = CacheRepository.GetItems<Comment>(GetOptionsString(options));
            if (comments != null)
                return comments;

            comments = CommentRepository.GetWithOptions(options);
            CacheRepository.Add(comments, GetOptionsString(options));
            return comments;
        }

        public void DeleteComment(int id, int newsPostId)
        {
            CommentRepository.Delete(id);
            UnitOfWork.Save();
            CacheRepository.DeleteByPartOfTheKey($"{CommentsByPostIdCache}-{newsPostId}");
        }

        public int Count()
        {
            return CommentRepository.Count();
        }

        public int GetCommentCountByPostId(int id)
        {
            var comments = CacheRepository.GetItems<Comment>($"{CommentsByPostIdCache}-{id}");
            if (comments != null)
                return comments.Count();

            return CommentRepository.GetCountByPostId(id);
        }

        private string GetOptionsString(OptionsCollectionById options)
        {
            return $"{CommentsByPostIdCache}-{options.TargetId}-{CommentsWithToolsCache}-{options.DateRange}-{options.SortType}-{options.Page}-{options.Search}";
        }
    }
}
