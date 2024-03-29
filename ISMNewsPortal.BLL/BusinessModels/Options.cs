﻿using System;

namespace ISMNewsPortal.BLL.BusinessModels
{
    public class Options
    {
        public int Page { get; set; }
        public int Pages { get; set; }
        public string Filter { get; set; }
        public DateRange DateRange { get; set; }
        public bool? Published { get; set; }
        public string SortType { get; set; }
        public string Search { get; set; }
        public bool? Reversed { get; set; }
        public bool Admin { get; set; }

        public Options()
        {
            Page = 1;
        }

        public Options(Options toolBar)
        {
            Pages = toolBar.Pages;
            Page = toolBar.Page;
            Search = toolBar.Search;
            SortType = toolBar.SortType;
            Filter = toolBar.Filter;
            Reversed = toolBar.Reversed;
            Admin = toolBar.Admin;
        }
    }
}
