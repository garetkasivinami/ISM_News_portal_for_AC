using ISMNewsPortal.BLL.Services;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using ISMNewsPortal.BLL.BusinessModels;
using ISMNewsPortal.BLL.Models;
using ISMNewsPortal.Models;

namespace ISMNewsPortal.Helpers
{
    public static class NewsPostHelper
    {
        public static void CreateNewsPost(NewsPost newsPost)
        {
            NewsPostService newsPostService = new NewsPostService();
            newsPostService.CreateNewsPost(newsPost);
        }

        public static void UpdateNewsPost(NewsPostEditModel editModel)
        {
            NewsPostService newsPostService = new NewsPostService();
            var newsPost = newsPostService.GetNewsPost(editModel.Id);
            editModel.PassToNewsPost(newsPost);
            newsPostService.UpdateNewsPost(newsPost);
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

            var newsPost = newsPostService.GetNewsPost(id);
            if (onlyVisible && (!newsPost.IsVisible || newsPost.PublicationDate > DateTime.Now))
                throw new Exception("Post isn`t visible!");

            var comments = commentService.GetCommentsByPostId(newsPost.Id);

            int commentsCount = comments.Count();
            int pages = commentsCount / Comment.CommentsInOnePage;

            if (commentsCount % Comment.CommentsInOnePage != 0)
                pages++;

            comments = comments.Skip((commentPage - 1) * Comment.CommentsInOnePage).Take(Comment.CommentsInOnePage).ToList();

            var commentsViewModel = new List<CommentViewModel>();
            foreach (Comment comment in comments)
            {
                commentsViewModel.Add(new CommentViewModel(comment));
            }
            return new NewsPostViewModel(newsPost, commentsViewModel, commentPage, pages, allowAdminActions);
        }

        public static NewsPostAdminCollection GenerateNewsPostAdminCollection(ToolsModel model)
        {
            NewsPostService newsPostService = new NewsPostService();
            AdminService adminService = new AdminService();
            CommentService commentService = new CommentService();

            var businessModel = model.ConvertToOptions();
            var newsPosts = newsPostService.GetNewsPostsWithAdminTools(businessModel);

            var newsPostAdminViews = new List<NewsPostAdminView>();
            foreach (NewsPost newsPost in newsPosts)
            {
                string newsPostAuthorName = adminService.GetAdmin(newsPost.AuthorId).Login;
                int commentCount = commentService.GetCommentCountByPostId(newsPost.Id);
                newsPostAdminViews.Add(new NewsPostAdminView(newsPost, newsPostAuthorName, commentCount));
            }
            model.Pages = businessModel.Pages;
            return new NewsPostAdminCollection(newsPostAdminViews, model);
        }

        public static NewsPostSimplifiedCollection GenerateNewsPostSimplifiedCollection(ToolsModel model)
        {
            NewsPostService newsPostService = new NewsPostService();
            CommentService commentService = new CommentService();

            var businessModel = model.ConvertToOptions();
            var newsPosts = newsPostService.GetNewsPostsWithTools(businessModel);

            var newsPostSimplifiedViews = new List<NewsPostSimplifiedView>();
            foreach (NewsPost newsPost in newsPosts)
            {
                int commentCount = commentService.GetCommentCountByPostId(newsPost.Id);
                newsPostSimplifiedViews.Add(new NewsPostSimplifiedView(newsPost, commentCount));
            }
            model.Pages = businessModel.Pages;
            return new NewsPostSimplifiedCollection(newsPostSimplifiedViews, model);

        }

        public static NewsPostAdminView GetNewsPostAdminView(int id)
        {
            NewsPostService newsPostService = new NewsPostService();
            AdminService adminService = new AdminService();

            var newsPost = newsPostService.GetNewsPost(id);
            string newsPostAuthorName = adminService.GetAdmin(newsPost.AuthorId).Login;
            int commentsCount = newsPostService.CommentsCount(id);
            return new NewsPostAdminView(newsPost, newsPostAuthorName, commentsCount);
        }

        public static NewsPostEditModel GetNewsPostEditModel(int id)
        {
            NewsPostService newsPostService = new NewsPostService();

            var newsPost = newsPostService.GetNewsPost(id);
            return new NewsPostEditModel(newsPost);
        }
    }
}
