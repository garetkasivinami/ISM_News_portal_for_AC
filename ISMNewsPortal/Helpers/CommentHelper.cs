using ISMNewsPortal.BLL.BusinessModels;
using ISMNewsPortal.BLL.Models;
using ISMNewsPortal.BLL.Services;
using ISMNewsPortal.Models;
using ISMNewsPortal.Models.Tools;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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

        public static string BuildErrorMessage(ModelStateDictionary modelState)
        {
            bool addNewLine = false;
            string result = "";
            if (modelState["UserName"].Errors.Any())
            {
                result += Language.Language.CommentAuthorNotValid;
                addNewLine = true;
            }
            if (addNewLine)
                result += Environment.NewLine;
            if (modelState["Text"].Errors.Any())
                result += Language.Language.CommentTextNotValid;
            return result;
        }

        public static CommentViewModelCollection GenerateCommentViewModelCollection(CommentToolsModel options)
        {
            CommentService commentService = new CommentService();
            NewsPostService newsPostService = new NewsPostService();
            var optionsBusiness = options.ConvertToOptionsCollectionById();
            var comments = commentService.GetCommentsWithTools(optionsBusiness);

            int commentsCount = comments.Count();

            var commentsViewModel = new List<CommentViewModel>();
            foreach (Comment comment in comments)
            {
                commentsViewModel.Add(new CommentViewModel(comment));
            }
            options.Pages = optionsBusiness.Pages;
            var newsPost = newsPostService.GetNewsPost(options.Id);
            string imagePath = FileModelActions.GetNameByIdFormated(newsPost.ImageId);
            return new CommentViewModelCollection(newsPost, imagePath, commentsViewModel, options, commentsCount);
        }
    }
}