using ISMNewsPortal.BLL.Services;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using ISMNewsPortal.BLL.BusinessModels;
using ISMNewsPortal.BLL.Models;
using ISMNewsPortal.Models;
using ISMNewsPortal.BLL.Exceptions;
using ISMNewsPortal.Models.Tools;
using System.Web;

namespace ISMNewsPortal.Helpers
{
    public static class NewsPostHelper
    {
        public static NewsPostService NewsPostService { get; private set; }

        static NewsPostHelper()
        {
            NewsPostService = new NewsPostService();
        }

        public static void CreateNewsPost(NewsPost newsPost)
        {
            NewsPostService.CreateNewsPost(newsPost);
        }

        public static void UpdateNewsPost(NewsPostEditModel editModel)
        {
            var newsPost = NewsPostService.GetNewsPost(editModel.Id);
            editModel.PassToNewsPost(newsPost);
            NewsPostService.UpdateNewsPost(newsPost);
        }

        public static void DeleteNewsPost(int id, HttpServerUtilityBase server)
        {
            var newsPost = NewsPostService.GetNewsPost(id);
            NewsPostService.DeleteNewsPost(id);
            FileModelActions.RemoveFile(newsPost.ImageId, server);
        }

        public static NewsPostViewModel GetNewsPostViewModelById(int id, int commentPage, bool allowAdminActions, bool onlyVisible = false)
        {
            var commentService = CommentHelper.CommentService;

            var newsPost = NewsPostService.GetNewsPost(id);
            if (onlyVisible && (!newsPost.IsVisible || newsPost.PublicationDate > DateTime.Now))
                throw new NewsPostNullException("Post isn`t visible!");

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
            return new NewsPostViewModel(newsPost, commentsViewModel, commentPage, pages, allowAdminActions, commentsCount);
        }

        public static NewsPostAdminCollection GenerateNewsPostAdminCollection(ToolsModel model)
        {
            var adminService = AdminHelper.AdminService;
            var commentService = CommentHelper.CommentService;

            var businessModel = model.ConvertToOptions();
            var newsPosts = NewsPostService.GetNewsPostsWithAdminTools(businessModel);

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
            var commentService = CommentHelper.CommentService;

            var businessModel = model.ConvertToOptions();
            var newsPosts = NewsPostService.GetNewsPostsWithTools(businessModel);

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
            var adminService = AdminHelper.AdminService;

            var newsPost = NewsPostService.GetNewsPost(id);
            string newsPostAuthorName = adminService.GetAdmin(newsPost.AuthorId).Login;
            int commentsCount = NewsPostService.CommentsCount(id);
            return new NewsPostAdminView(newsPost, newsPostAuthorName, commentsCount);
        }

        public static NewsPostEditModel GetNewsPostEditModel(int id)
        {
            var newsPost = NewsPostService.GetNewsPost(id);
            return new NewsPostEditModel(newsPost);
        }
    }
}
