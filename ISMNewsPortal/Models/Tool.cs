using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISMNewsPortal.Models
{
    public class ToolBarModel
    {
        public ToolBarModel()
        {
        }
        public ToolBarModel(ToolBarModel toolBar)
        {
            Pages = toolBar.Pages;
            Page = toolBar.Page;
            Search = toolBar.Search;
            TypeSearch = toolBar.TypeSearch;
            Filter = toolBar.Filter;
            Page = toolBar.Page;
        }
        public int Page { get; set; }
        public int Pages { get; set; }
        public string Filter { get; set; }
        public string SortType { get; set; }
        public string Search { get; set; }
        public string TypeSearch { get; set; }
    }
}