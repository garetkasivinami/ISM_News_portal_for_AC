using System.Web.Mvc;

namespace ISMNewsPortal.Models.Tools
{
    public class ToolsModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var valueProvider = bindingContext.ValueProvider;
            object toolsModel = CreateNewToolsObject();
            BindFields(toolsModel, valueProvider);
            return toolsModel;
        }

        protected virtual void BindFields(object target, IValueProvider valueProvider)
        {
            var toolsModel = target as ToolsModel;
            toolsModel.Page = (int)(valueProvider.GetValue("Page")?.ConvertTo(typeof(int)) ?? 1) - 1;
            toolsModel.Pages = (int)(valueProvider.GetValue("Pages")?.ConvertTo(typeof(int)) ?? 1);
            toolsModel.Reversed = (bool?)(valueProvider.GetValue("Reversed")?.ConvertTo(typeof(bool)));
            toolsModel.Search = (string)(valueProvider.GetValue("Search")?.ConvertTo(typeof(string)));
            toolsModel.SortType = (string)(valueProvider.GetValue("SortType")?.ConvertTo(typeof(string)));
            toolsModel.Filter = (string)(valueProvider.GetValue("Filter")?.ConvertTo(typeof(string)));
        }

        protected virtual object CreateNewToolsObject()
        {
            return new ToolsModel();
        }
    }
}