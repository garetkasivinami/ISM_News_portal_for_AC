﻿using ISMNewsPortal.BLL.BusinessModels;
using ISMNewsPortal.BLL.DTO;
using ISMNewsPortal.BLL.Services;
using ISMNewsPortal.Mappers;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISMNewsPortal.Models
{
    public static class CommentHelper
    {
        public static int CreateComment(Comment comment)
        {
            using(CommentService commentService = new CommentService()) {
                var commentDTO = DTOMapper.MapCommentDTO(comment);
                return commentService.CreateComment(commentDTO);
            }
        }

        public static void UdpateComment(Comment comment)
        {
            using (CommentService commentService = new CommentService())
            {
                var commentDTO = DTOMapper.MapCommentDTO(comment);
                commentService.UpdateComment(commentDTO);
            }
        }

        public static void DeleteComment(int id)
        {
            using (CommentService commentService = new CommentService())
            {
                commentService.DeleteComment(id);
            }
        }

        public static CommentViewModelCollection GenerateCommentViewModelCollection(int postId)
        {
            using (CommentService commentService = new CommentService())
            {
                var commentsDTO = commentService.GetCommentsByPostId(postId);
                var comments = DTOMapper.MapComments(commentsDTO);

                int commentsCount = comments.Count();

                var commentsViewModel = new List<CommentViewModel>();
                foreach (Comment comment in comments)
                {
                    commentsViewModel.Add(new CommentViewModel(comment));
                }
                NewsPost newsPost;
                using (NewsPostService newsPostService = new NewsPostService(commentService))
                {
                    var newsPostDTO = newsPostService.GetNewsPost(postId);
                    newsPost = DTOMapper.NewsPostMapper.Map<NewsPostDTO, NewsPost>(newsPostDTO);
                }
                string imagePath = FileModelActions.GetNameByIdFormated(newsPost.ImageId);
                return new CommentViewModelCollection(newsPost, imagePath, commentsViewModel, new ToolBarModel(), commentsCount);
            }
        }
    }
}