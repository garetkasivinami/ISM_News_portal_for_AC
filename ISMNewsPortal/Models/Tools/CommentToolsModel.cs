using ISMNewsPortal.BLL.BusinessModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ISMNewsPortal.Models.Tools
{
    [ModelBinder(typeof(CommentToolsModelBinder))]
    public class CommentToolsModel : ToolsModel
    {
        public int Id { get; set; }

        public CommentToolsModel()
        {

        }

        public CommentToolsModel(CommentToolsModel toolBar) : base(toolBar)
        {
            Id = toolBar.Id;
        }

        public OptionsCollectionById ConvertToOptionsCollectionById()
        {
            OptionsCollectionById options = new OptionsCollectionById();
            CopyOptions(options);
            options.TargetId = Id;
            return options;
        }
    }
}