﻿using ISMNewsPortal.BLL.Services;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using ISMNewsPortal.BLL.BusinessModels;
using ISMNewsPortal.BLL.DTO;
using ISMNewsPortal.BLL.Infrastructure;
using static ISMNewsPortal.BLL.Mappers.Automapper;
using ISMNewsPortal.Models;

namespace ISMNewsPortal.Helpers
{
    public static class NewsPostHelper
    {
        public static void CreateNewsPost(NewsPostDTO newsPost)
        {
            NewsPostService newsPostService = new NewsPostService();

            var newsPostDTO = MapToNewsPostDTO(newsPost);
            newsPostService.CreateNewsPost(newsPostDTO);
        }

        public static void UpdateNewsPost(NewsPostDTO newsPost)
        {
            NewsPostService newsPostService = new NewsPostService();
 
            var newsPostDTO = MapToNewsPostDTO(newsPost);
            newsPostService.UpdateNewsPost(newsPostDTO);
        }

        public static void DeleteNewsPost(int id)
        {
            NewsPostService newsPostService = new NewsPostService();

            newsPostService.DeleteNewsPost(id);
        }

        public static NewsPostViewModel GetNewsPostViewModelById(int id, int commentPage, bool allowAdminActions, bool onlyVisible = false)
        {
            NewsPostService newsPostService = new NewsPostService();
            CommentService commentService = new CommentService();

            var newsPostDTO = newsPostService.GetNewsPost(id);
            var newsPost = MapFromNewsPostDTO<NewsPostDTO>(newsPostDTO);
            if (onlyVisible && (!newsPost.IsVisible || newsPost.PublicationDate > DateTime.Now))
                throw new Exception("Post isn`t visible!");

            var commentDTOs = commentService.GetCommentsByPostId(newsPost.Id);
            var comments = MapFromCommentDTOList<CommentDTO>(commentDTOs);

            int commentsCount = comments.Count();
            int pages = commentsCount / CommentDTO.CommentsInOnePage;
            if (commentsCount % CommentDTO.CommentsInOnePage != 0)
                pages++;

            comments = comments.Skip(commentPage * CommentDTO.CommentsInOnePage).Take(CommentDTO.CommentsInOnePage).ToList();

            var commentsViewModel = new List<CommentViewModel>();
            foreach (CommentDTO comment in comments)
            {
                commentsViewModel.Add(new CommentViewModel(comment));
            }
            return new NewsPostViewModel(newsPost, commentsViewModel, commentPage, pages, allowAdminActions);
        }

        public static NewsPostAdminCollection GenerateNewsPostAdminCollection(ToolBarModel model)
        {
            NewsPostService newsPostService = new NewsPostService();
            AdminService adminService = new AdminService();
            CommentService commentService = new CommentService();

            var toolsDTO = MapToToolsDTO(model);
            var newsPostsDTO = newsPostService.GetNewsPostsWithAdminTools(toolsDTO);
            var newsPosts = MapFromNewsPostDTOList<NewsPostDTO>(newsPostsDTO);

            var newsPostAdminViews = new List<NewsPostAdminView>();
            foreach (NewsPostDTO newsPost in newsPosts)
            {
                string newsPostAuthorName = adminService.GetAdmin(newsPost.AuthorId).Login;
                int commentCount = commentService.GetCommentCountByPostId(newsPost.Id);
                newsPostAdminViews.Add(new NewsPostAdminView(newsPost, newsPostAuthorName, commentCount));
            }
            model.Pages = toolsDTO.Pages;
            return new NewsPostAdminCollection(newsPostAdminViews, model);
        }

        public static NewsPostSimplifiedCollection GenerateNewsPostSimplifiedCollection(ToolBarModel model)
        {
            NewsPostService newsPostService = new NewsPostService();
            CommentService commentService = new CommentService();

            var modelDTO = MapToToolsDTO(model);
            var newsPostsDTO = newsPostService.GetNewsPostsWithTools(modelDTO);
            var newsPosts = MapFromNewsPostDTOList<NewsPostDTO>(newsPostsDTO);

            var newsPostSimplifiedViews = new List<NewsPostSimplifiedView>();
            foreach (NewsPostDTO newsPost in newsPosts)
            {
                int commentCount = commentService.GetCommentCountByPostId(newsPost.Id);
                newsPostSimplifiedViews.Add(new NewsPostSimplifiedView(newsPost, commentCount));
            }
            model.Pages = modelDTO.Pages;
            return new NewsPostSimplifiedCollection(newsPostSimplifiedViews, model);

        }

        public static NewsPostAdminView GetNewsPostAdminView(int id)
        {
            NewsPostService newsPostService = new NewsPostService();
            AdminService adminService = new AdminService();

            var newsPostDTO = newsPostService.GetNewsPost(id);
            var newsPost = MapFromNewsPostDTO<NewsPostDTO>(newsPostDTO);
            string newsPostAuthorName = adminService.GetAdmin(newsPost.AuthorId).Login;
            int commentsCount = newsPostService.CommentsCount(id);
            return new NewsPostAdminView(newsPost, newsPostAuthorName, commentsCount);
        }

        public static NewsPostEditModel GetNewsPostEditModel(int id)
        {
            NewsPostService newsPostService = new NewsPostService();

            var newsPostDTO = newsPostService.GetNewsPost(id);
            var newsPost = MapFromNewsPostDTO<NewsPostDTO>(newsPostDTO);
            return new NewsPostEditModel(newsPost);
        }
    }
}