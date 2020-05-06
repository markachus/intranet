using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace IntranetCore.Data.Helpers
{
    public class PagedList<T> : List<T>
    {

        public int TotalCount { get; private set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }

        public bool HasNext { get => CurrentPage < TotalPages; }
        public bool HasPrevious { get => CurrentPage > 1; }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="items">Elements of a page</param>
        /// <param name="count">Total of elements in the collection without pagination</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="pageNumber">Current page number</param>
        private PagedList(List<T> items, int count, int pageSize, int pageNumber)
        {
            TotalCount = count;
            this.PageSize = pageSize;
            this.CurrentPage = pageNumber;
            this.TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            AddRange(items);
        }


        public async static Task<PagedList<T>> Create(IQueryable<T> source, int pageNumber, int pageSize) {

            var count = source.Count();
            var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PagedList<T>(items, count, pageSize, pageNumber);
        }
    }
}