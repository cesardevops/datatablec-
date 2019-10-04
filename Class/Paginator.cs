using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataTablesjs.Class
{
    public class Paginator
    {
        public int queryRecordCount
        {
            get;
            set;
        }
        public int pageIndex
        {
            get;
            set;
        }
        public int totalRecordCount
        {
            get;
            set;
        }
        public string SearchFilter
        {
            get;
            set;
        }
        public string SortBy
        {
            get;
            set;
        }
        public object records
        {
            get;
            set;
        }
    }
}