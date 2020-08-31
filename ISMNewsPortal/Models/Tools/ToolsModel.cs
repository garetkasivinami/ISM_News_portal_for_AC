using ISMNewsPortal.BLL.BusinessModels;
using System;
using System.Web.Mvc;

namespace ISMNewsPortal.Models.Tools
{
    [ModelBinder(typeof(ToolsModelBinder))]
    public class ToolsModel
    {
        public int Page { get; set; }
        public int Pages { get; set; }
        public string Filter { get; set; }
        public string SortType { get; set; }
        public string Search { get; set; }
        public bool? Reversed { get; set; }
        public bool Admin { get; set; }

        public ToolsModel()
        {
            Page = 1;
        }

        public ToolsModel(ToolsModel toolBar)
        {
            Pages = toolBar.Pages;
            Page = toolBar.Page;
            Search = toolBar.Search;
            SortType = toolBar.SortType;
            Filter = toolBar.Filter;
            Reversed = toolBar.Reversed;
            Admin = toolBar.Admin;
        }
        public Options ConvertToOptions()
        {
            Options options = new Options();
            CopyOptions(options);
            return options;
        }

        protected void CopyOptions(Options options)
        {
            options.Admin = Admin;
            options.Page = Page;
            options.Pages = Pages;
            options.Reversed = Reversed;
            options.Search = Search;
            options.SortType = SortType;
            SetFilter(options);
        }

        private void SetFilter(Options target)
        {
            var filter = Filter?.ToLower();
            var currentDate = DateTime.Now;

            switch(filter)
            {
                case "today":
                    target.DateRange = new DateRange(currentDate.Date, currentDate.AddDays(1).Date);
                    break;
                case "yesterday":
                    target.DateRange = new DateRange(currentDate.AddDays(-1).Date, currentDate.Date);
                    break;
                case "week":
                    target.DateRange = new DateRange(currentDate.AddDays(-6).Date, currentDate.AddDays(1).Date);
                    break;
                case "onlypublished":
                    target.Published = true;
                    break;
                case "notpublished":
                    target.Published = false;
                    break;
                default:
                    break;
            }
        }
    }
}