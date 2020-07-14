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

        public static string FilterAll()
        {
            return "1 = 1";
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
                default:
                    filterFunc = FilterAll();
                    break;
            }
            return filterFunc;
        }

        public static string GetSortSqlString(string sortType)
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
            return sortString;
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
            return "Name LIKE :searchName AND Description LIKE :searchName ";
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
