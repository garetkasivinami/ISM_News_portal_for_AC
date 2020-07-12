using ISMNewsPortal.BLL.BusinessModels;
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
        public static CommentViewModelCollection GenerateCommentViewModelCollection(ToolBarModel model)
        {
            using (CommentService commentService = new CommentService())
            {
                var toolBarDTO = DTOMapper.ToolsMapperToDTO.Map<ToolBarModel, ToolsDTO>(model);
                var commentsDTO = commentService.GetCommentsWithTools(toolBarDTO);
                var comments = DTOMapper.CommentMapper.Map<IEnumerable<CommentDTO>, List<Comment>>(commentsDTO);

                int commentsCount = comments.Count();

                var commentsViewModel = new List<CommentViewModel>();
                foreach(Comment comment in comments)
                {
                    commentsViewModel.Add(new CommentViewModel(comment));
                }
                return new CommentViewModelCollection(commentsViewModel, model, commentsCount);
            }
        }
    }
}