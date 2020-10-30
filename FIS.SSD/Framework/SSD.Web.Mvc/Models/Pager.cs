using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SSD.Web.Mvc.Models
{
    public class Pager<T> : List<T>
    {
        public System.Web.Routing.RouteValueDictionary RouteValues { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int PagesToShow { get; set; }
        public long TotalCount { get; set; }
        public string Action { get; set; }
        public Pager(IEnumerable<T> dataSource, int pageIndex, int pageSize, long totalCount, string action = "Index", int pagesToShow = 4)
        {
            Action = action;
            TotalCount = totalCount;
            CurrentPage = pageIndex;
            PageSize = pageSize;
            TotalPages = (int)Math.Ceiling(TotalCount / (double)PageSize);
            PagesToShow = pagesToShow;
            this.AddRange(dataSource);
        }
        public Pager(IQueryable<T> dataSource, int pageIndex, int pageSize, string action = "Index", int pagesToShow = 4)//long totalCount,
        {
            Action = action;
            TotalCount = dataSource.LongCount() + 1;
            CurrentPage = pageIndex;
            PageSize = pageSize;
            TotalPages = (int)Math.Ceiling(TotalCount / (double)PageSize);
            PagesToShow = pagesToShow;
            this.AddRange(dataSource.Skip((pageIndex - 1) * pageSize).Take(pageSize));
        }
    }
}