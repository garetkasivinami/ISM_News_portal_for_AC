﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISMNewsPortal.Models
{
    public class ToolBarModel
    {
        public int Page { get; set; }
        public int Pages { get; set; }
        public string Filter { get; set; }
        public string SortType { get; set; }
        public string Search { get; set; }
        public bool? Reversed { get; set; }

        public ToolBarModel()
        {
        }

        public ToolBarModel(ToolBarModel toolBar)
        {
            Pages = toolBar.Pages;
            Page = toolBar.Page;
            Search = toolBar.Search;
            SortType = toolBar.SortType;
            Filter = toolBar.Filter;
            Reversed = toolBar.Reversed;
        }
    }
}