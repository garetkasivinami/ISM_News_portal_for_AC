using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISMNewsPortal.DAL.ToolsLogic
{
    public static class NewsPostToolsLogic
    {
        public static string FilterToday()
        {
            return $"PublicationDate >= CONVERT(DATETIME, '{DateTime.Now.Year}.{DateTime.Now.Month}.{DateTime.Now.Day}')";
        }

        public static string FilterYesterday()
        {
            DateTime yesterday = DateTime.Now.AddDays(-1);
            return $"PublicationDate >= CONVERT(DATETIME, '{yesterday.Year}.{yesterday.Month}.{yesterday.Day}') AND CreatedDate < CONVERT(DATETIME, '{DateTime.Now.Year}.{DateTime.Now.Month}.{DateTime.Now.Day}')";
        }

        public static string FilterWeek()
        {
            DateTime week = DateTime.Now.AddDays(-7);
            return $"PublicationDate >= CONVERT(DATETIME, '{week.Year}.{week.Month}.{week.Day}')";
        }

        public static string FilterDontPublished()
        {
            DateTime week = DateTime.Now.AddDays(-7);
            return $"PublicationDate > GETDATE() OR IsVisible = false";
        }
        public static string FilterPublished()
        {
            DateTime week = DateTime.Now.AddDays(-7);
            return $"PublicationDate <= GETDATE() AND IsVisible = true";
        }

        public static string FilterAll()
        {
            return "1 = 1";
        }

        public static DateTime GetMinFilterDate(string filter)
        {
            var currentDate = DateTime.Now;
            switch (filter)
            {
                case "today":
                    return currentDate.Date;
                case "yesterday":
                    return currentDate.Date.AddDays(-1);
                case "week":
                    return currentDate.Date.AddDays(-7);
                default:
                    return new DateTime(0);
            }
        }

        public static DateTime GetMaxFilterDate(string filter)
        {
            var currentDate = DateTime.Now;
            switch (filter)
            {
                case "today":
                    return currentDate;
                case "yesterday":
                    return currentDate.Date.AddSeconds(-1);
                case "week":
                    return currentDate;
                default:
                    return currentDate;
            }
        }

        public static string GetFilterSqlString(string filter)
        {
            string filterFunc;
            switch (filter)
            {
                case "today":
                    filterFunc = FilterToday();
                    break;
                case "yesterday":
                    filterFunc = FilterYesterday();
                    break;
                case "week":
                    filterFunc = FilterWeek();
                    break;
                case "only published":
                    filterFunc = FilterPublished();
                    break;
                case "not published":
                    filterFunc = FilterDontPublished();
                    break;
                default:
                    filterFunc = FilterAll();
                    break;
            }
            return filterFunc;
        }

        public static string GetSortSqlString(string sortType, bool reversed)
        {
            string sortString;
            switch (sortType)
            {
                case "name":
                    sortString = "@Name";
                    break;
                case "description":
                    sortString = "@Description";
                    break;
                default:
                    sortString = "@PublicationDate DESC";
                    break;
            }
            if (reversed)
                sortString += " DESC";
            return sortString;
        }

        public static string GetSortFieldName(string sortType)
        {
            switch (sortType)
            {
                case "name":
                    return "Name";
                case "description":
                    return "Description";
                default:
                    return "PublicationDate";
            }
        }

        public static string GetAdminSortSqlString(string sortType, bool reversed)
        {
            string sortString;
            switch (sortType)
            {
                case "id":
                    sortString = "@Id";
                    break;
                case "name":
                    sortString = "@Name";
                    break;
                case "description":
                    sortString = "@Description";
                    break;
                case "editDate":
                    sortString = "@EditDate";
                    break;
                case "Author":
                    sortString = "@AuthorId";
                    break;
                case "publishDate":
                    reversed = !reversed;
                    sortString = "@PublicationDate";
                    break;
                case "visibility":
                    sortString = "@IsVisible";
                    break;
                default:
                    reversed = !reversed;
                    sortString = "@CreatedDate";
                    break;
            }
            if (reversed)
                sortString += " DESC";
            return sortString;
        }

        public static string GetSearchSqlString()
        {
            return "(Name LIKE :searchName OR Description LIKE :searchName) ";
        }

        public static IQuery GetSqlQuerry(ISession session, string sortType, string filter, string search, string searchString)
        {
            search = search ?? "_";
            return session.CreateQuery($"from NewsPost where {filter} AND {searchString} AND IsVisible = 1 AND PublicationDate <= GetDate() ORDER BY {sortType}").
                    SetParameter("searchName", $"%{search}%");
        }

        public static IQuery GetSqlQuerryAdmin(ISession session, string sortType, string filter, string search, string searchString)
        {
            search = search ?? "_";
            return session.CreateQuery($"from NewsPost where {filter} AND {searchString} ORDER BY {sortType}").
                    SetParameter("searchName", $"%{search}%");
        }
    }
}
