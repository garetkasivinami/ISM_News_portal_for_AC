using AutoMapper;
using ISMNewsPortal.BLL.BusinessModels;
using ISMNewsPortal.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISMNewsPortal.BLL.Exceptions;
using static ISMNewsPortal.BLL.UnitOfWorkManager;

namespace ISMNewsPortal.BLL.Services
{
    public class CommentService
    {

        public IEnumerable<Comment> GetComments()
        {
            return CommentRepository.GetAll();
        }

        public Comment GetComment(int id)
        {
            var comment = CommentRepository.Get(id);
            if (comment == null)
                throw new CommentNullException();
            return comment;
        }

        public IEnumerable<Comment> GetCommentsByPostId(int id)
        {
            var comments = CommentRepository.GetByPostId(id).Reverse();
            return comments;
        }

        public void UpdateComment(Comment commentDTO)
        {
            CommentRepository.Update(commentDTO);
            UnitOfWork.Save();
        }

        public int CreateComment(Comment commentDTO)
        {
            int id = CommentRepository.Create(commentDTO);
            UnitOfWork.Save();
            return id;
        }

        public IEnumerable<Comment> GetCommentsWithTools(OptionsCollectionById options)
        {
            var comments = CommentRepository.GetWithOptions(options);
            return comments;
        }

        public void DeleteComment(int id)
        {
            CommentRepository.Delete(id);
            UnitOfWork.Save();
        }

        public int Count()
        {
            return CommentRepository.Count();
        }

        public int GetCommentCountByPostId(int id)
        {
            return CommentRepository.GetCountByPostId(id);
        }

        public static IEnumerable<Comment> SortBy(IEnumerable<Comment> items, string sortType)
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
            return items;
        }

        public static IEnumerable<Comment> SortByReversed(IEnumerable<Comment> items, string sortType)
        {
            sortType = sortType?.ToLower();
            switch (sortType)
            {
                case "id":
                    items = items.OrderByDescending(u => u.Id);
                    break;
                case "username":
                    items = items.OrderByDescending(u => u.UserName);
                    break;
                case "commenttext":
                    items = items.OrderByDescending(u => u.Text);
                    break;
                default:
                    items = items.OrderByDescending(u => u.Date);
                    break;
            }
            return items;
        }
    }
}
