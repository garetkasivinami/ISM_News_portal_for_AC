﻿using ISMNewsPortal.BLL.Services;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using ISMNewsPortal.Mappers;
using ISMNewsPortal.BLL.BusinessModels;
using ISMNewsPortal.BLL.DTO;
using ISMNewsPortal.BLL.Infrastructure;

namespace ISMNewsPortal.Models
{
    public static class NewsPostHelper
    {
        public static void CreateNewsPost(NewsPost newsPost)
        {
            using (NewsPostService newsPostService = new NewsPostService())
            {
                var newsPostDTO = DTOMapper.MapNewsPostDTO(newsPost);
                newsPostService.CreateNewsPost(newsPostDTO);
            }
        }

        public static void UpdateNewsPost(NewsPost newsPost)
        {
            using (NewsPostService newsPostService = new NewsPostService())
            {
                var newsPostDTO = DTOMapper.MapNewsPostDTO(newsPost);
                newsPostService.UpdateNewsPost(newsPostDTO);
            }
        }

        public static void DeleteNewsPost(int id)
        {
            using (NewsPostService newsPostService = new NewsPostService())
            {
                newsPostService.DeleteNewsPost(id);
            }
        }

        public static NewsPostViewModel GetNewsPostViewModelById(int id, int page, bool moderActions, bool checkVisibility = false)
        {
            using (NewsPostService newsPostService = new NewsPostService())
            {
                var newsPostDTO = newsPostService.GetNewsPost(id);
                var newsPost = DTOMapper.MapNewsPost(newsPostDTO);
                if (checkVisibility && (!newsPost.IsVisible || newsPost.PublicationDate > DateTime.Now))
                    throw ExceptionGenerator.GenerateException("Post isn`t visivle!", "NewsPostHelper.GetNewsPostViewModelById(int id, int page, bool moderActions, bool checkVisibility = false)", 
                        $"id: {id}", $"page: {page}", $"moderActions: {moderActions}", $"checkVisibility: {checkVisibility}");

                IEnumerable<CommentDTO> commentDTOs;
                List<Comment> comments;
                using (CommentService commentService = new CommentService(newsPostService))
                {
                    commentDTOs = commentService.GetCommentsByPostId(newsPost.Id);
                    comments = DTOMapper.MapComments(commentDTOs);
                }

                int commentsCount = comments.Count();
                int pages = commentsCount / Comment.CommentsInOnePage;
                if (commentsCount % Comment.CommentsInOnePage != 0)
                    pages++;

                var commentsViewModel = new List<CommentViewModel>();
                foreach(Comment comment in comments)
                {
                    commentsViewModel.Add(new CommentViewModel(comment));
                }
                return new NewsPostViewModel(newsPost, commentsViewModel, page, pages, moderActions);
            }
        }

        public static NewsPostAdminCollection GenerateNewsPostAdminCollection(ToolBarModel model)
        {
            using (NewsPostService newsPostService = new NewsPostService())
            {
                var toolsDTO = DTOMapper.ToolsMapperToDTO.Map<ToolBarModel, ToolsDTO>(model);
                var newsPostsDTO = newsPostService.GetNewsPostsWithAdminTools(toolsDTO);
                var newsPosts = DTOMapper.MapNewsPosts(newsPostsDTO);


                var newsPostAdminViews = new List<NewsPostAdminView>();
                using (AdminService adminService = new AdminService(newsPostService))
                {
                    foreach (NewsPost newsPost in newsPosts)
                    {
                        string newsPostAuthorName = adminService.GetAdmin(newsPost.AuthorId).Login;
                        int commentCount;
                        using (CommentService commentService = new CommentService(adminService))
                        {
                            commentCount = commentService.GetCommentCountByPostId(newsPost.Id);
                        }
                        newsPostAdminViews.Add(new NewsPostAdminView(newsPost, newsPostAuthorName, commentCount));
                    }
                }
                model.Pages = toolsDTO.Pages;
                return new NewsPostAdminCollection(newsPostAdminViews, model);
            }
        }

        public static NewsPostSimplifiedCollection GenerateNewsPostSimplifiedCollection(ToolBarModel model)
        {
            using (NewsPostService newsPostService = new NewsPostService())
            {
                var modelDTO = DTOMapper.ToolsMapperToDTO.Map<ToolBarModel, ToolsDTO>(model);
                var newsPostsDTO = newsPostService.GetNewsPostsWithTools(modelDTO);
                var newsPosts = DTOMapper.MapNewsPosts(newsPostsDTO);

                var newsPostSimplifiedViews = new List<NewsPostSimplifiedView>();
                using (CommentService commentService = new CommentService(newsPostService))
                {
                    foreach (NewsPost newsPost in newsPosts)
                    {
                        int commentCount = commentService.GetCommentCountByPostId(newsPost.Id);
                        newsPostSimplifiedViews.Add(new NewsPostSimplifiedView(newsPost, commentCount));
                    }
                }
                model.Pages = modelDTO.Pages;
                return new NewsPostSimplifiedCollection(newsPostSimplifiedViews, model);
            }
        }

        public static NewsPostAdminView GetNewsPostAdminView(int id)
        {
            using (NewsPostService newsPostService = new NewsPostService())
            {
                var newsPostDTO = newsPostService.GetNewsPost(id);
                var newsPost = DTOMapper.MapNewsPost(newsPostDTO);
                string newsPostAuthorName;
                using (AdminService adminService = new AdminService(newsPostService))
                {
                    newsPostAuthorName = adminService.GetAdmin(newsPost.AuthorId).Login;
                }
                int commentsCount = newsPostService.CommentsCount(id);
                var newsPostAdminView = new NewsPostAdminView(newsPost, newsPostAuthorName, commentsCount);
                return newsPostAdminView;
            }
        }

        public static NewsPostEditModel GetNewsPostEditModel(int id)
        {
            using (NewsPostService newsPostService = new NewsPostService())
            {
                var newsPostDTO = newsPostService.GetNewsPost(id);
                var newsPost = DTOMapper.MapNewsPost(newsPostDTO);
                var newsPostEditModel = new NewsPostEditModel(newsPost);
                return newsPostEditModel;
            }
        }
    }
}
