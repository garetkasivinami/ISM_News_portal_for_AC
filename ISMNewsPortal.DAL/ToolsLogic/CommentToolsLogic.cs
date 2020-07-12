using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISMNewsPortal.DAL.ToolsLogic
{
    public static class CommentToolsLogic
    {
        public static string FilterToday()
        {
            return $"Date >= CONVERT(DATETIME, '{DateTime.Now.Year}.{DateTime.Now.Month}.{DateTime.Now.Day}')";
        }

        public static string FilterYesterday()
        {
            DateTime yesterday = DateTime.Now.AddDays(-1);
            return $"Date >= CONVERT(DATETIME, '{yesterday.Year}.{yesterday.Month}.{yesterday.Day}') AND Date < CONVERT(DATETIME, '{DateTime.Now.Year}.{DateTime.Now.Month}.{DateTime.Now.Day}')";
        }

        public static string FilterWeek()
        {
            DateTime week = DateTime.Now.AddDays(-7);
            return $"Date >= CONVERT(DATETIME, '{week.Year}.{week.Month}.{week.Day}')";
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

        public static string GetSortSqlString(string sortType, bool reversed)
        {
            string sortString;
            switch (sortType)
            {
                case "id":
                    sortString = "@Id";
                    break;
                case "username":
                    sortString = "@UserName";
                    break;
                case "text":
                    sortString = "@text";
                    break;
                case "newsPost":
                    sortString = "@NewsPostId";
                    break;
                default:
                    reversed = !reversed;
                    sortString = "@Date";
                    break;
            }
            if (reversed)
                sortString += " DESC";
            return sortString;
        }

        public static string GetSearchSqlString(string searchType)
        {
            string searchString;
            switch (searchType)
            {
                case "text":
                    searchString = "Text LIKE :searchName ";
                    break;
                default:
                    searchString = "UserName LIKE :searchName ";
                    break;
            }
            return searchString;
        }

        public static IQuery GetSqlQuerry(ISession session, string sortType, string filter, string search, string searchString)
        {
            search = search ?? "_";
            return session.CreateQuery($"from Comment where {filter} AND {searchString} ORDER BY {sortType}").
                    SetParameter("searchName", $"%{search}%");
        }
    }
}
