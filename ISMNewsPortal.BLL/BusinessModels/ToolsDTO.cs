using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISMNewsPortal.BLL.BusinessModels
{
    public class ToolsDTO
    {
        public int Page { get; set; }
        public int Pages { get; set; }
        public string Filter { get; set; }
        public string SortType { get; set; }
        public string Search { get; set; }
        public string TypeSearch { get; set; }
        public bool? Reversed { get; set; }
    }
}
