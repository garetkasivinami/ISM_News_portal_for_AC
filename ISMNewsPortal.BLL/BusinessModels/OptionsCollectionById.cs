using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISMNewsPortal.BLL.BusinessModels
{
    public class OptionsCollectionById : Options
    {
        public int TargetId { get; set; }
        public int CommentsCount { get; set; }
    }
}
