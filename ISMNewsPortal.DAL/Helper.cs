using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISMNewsPortal.DAL
{
    public static class Helper
    {
        public static IEnumerable<T> CutIEnumarable<T>(IEnumerable<T> target, int startIndex, int count)
        {
            return target.Skip(startIndex).Take(count);
        }
        public static IEnumerable<T> CutIEnumarable<T>(int page, int multiplier, IEnumerable<T> target)
        {
            return CutIEnumarable(target, page * multiplier, multiplier);
        }

        public static int CalculatePages(int count, int countInOnePage)
        {
            int pages = count / countInOnePage;
            if (count % countInOnePage != 0)
            {
                pages++;
            }
            return pages;
        }
    }
}
