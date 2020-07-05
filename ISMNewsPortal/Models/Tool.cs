using System;
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
    }
}