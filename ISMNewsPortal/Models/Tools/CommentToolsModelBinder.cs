using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ISMNewsPortal.Models.Tools
{
    public class CommentToolsModelBinder : ToolsModelBinder
    {
        protected override void BindFields(object target, IValueProvider valueProvider)
        {
            var toolsModel = target as CommentToolsModel;
            base.BindFields(target, valueProvider);
            toolsModel.Id = (int)valueProvider.GetValue("id").ConvertTo(typeof(int));
        }
        protected override object CreateNewToolsObject()
        {
            return new CommentToolsModel();
        }
    }
}