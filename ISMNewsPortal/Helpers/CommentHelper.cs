using ISMNewsPortal.BLL.BusinessModels;
using ISMNewsPortal.BLL.Models;
using ISMNewsPortal.BLL.Services;
using ISMNewsPortal.Models;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static ISMNewsPortal.BLL.Mappers.Automapper;

namespace ISMNewsPortal.Helpers
{
    public static class CommentHelper
    {
        public static int CreateComment(Comment comment)
        {
            CommentService commentService = new CommentService();
            return commentService.CreateComment(comment);
        }

        public static void UdpateComment(Comment comment)
        {
            CommentService commentService = new CommentService();
            commentService.UpdateComment(comment);
        }

        public static void DeleteComment(int id)
        {
            CommentService commentService = new CommentService();
            commentService.DeleteComment(id);
        }

        public static CommentViewModelCollection GenerateCommentViewModelCollection(int postId)
        {
            CommentService commentService = new CommentService();
            NewsPostService newsPostService = new NewsPostService();
            var comments = commentService.GetCommentsByPostId(postId);

            int commentsCount = comments.Count();

            var commentsViewModel = new List<CommentViewModel>();
            foreach (Comment comment in comments)
            {
                commentsViewModel.Add(new CommentViewModel(comment));
            }
            var newsPost = newsPostService.GetNewsPost(postId);
            string imagePath = FileModelActions.GetNameByIdFormated(newsPost.ImageId);
            return new CommentViewModelCollection(newsPost, imagePath, commentsViewModel, new Options(), commentsCount);
        }
    }
}