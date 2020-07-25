using ISMNewsPortal.BLL.BusinessModels;
using ISMNewsPortal.BLL.DTO;
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
        public static int CreateComment(CommentDTO comment)
        {
            CommentService commentService = new CommentService();
            var commentDTO = MapToCommentDTO(comment);
            return commentService.CreateComment(commentDTO);
        }

        public static void UdpateComment(CommentDTO comment)
        {
            CommentService commentService = new CommentService();
            var commentDTO = MapToCommentDTO(comment);
            commentService.UpdateComment(commentDTO);
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
            var commentsDTO = commentService.GetCommentsByPostId(postId);
            var comments = MapFromCommentDTOList<CommentDTO>(commentsDTO);

            int commentsCount = comments.Count();

            var commentsViewModel = new List<CommentViewModel>();
            foreach (CommentDTO comment in comments)
            {
                commentsViewModel.Add(new CommentViewModel(comment));
            }
            var newsPostDTO = newsPostService.GetNewsPost(postId);
            var newsPost = MapFromNewsPostDTO<NewsPostDTO>(newsPostDTO);
            string imagePath = FileModelActions.GetNameByIdFormated(newsPost.ImageId);
            return new CommentViewModelCollection(newsPost, imagePath, commentsViewModel, new ToolBarModel(), commentsCount);
        }
    }
}