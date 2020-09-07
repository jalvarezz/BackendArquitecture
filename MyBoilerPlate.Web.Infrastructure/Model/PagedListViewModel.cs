using System;
using System.Collections.Generic;

namespace MyBoilerPlate.Web.Infrastructure.Models
{
    public class PagedListViewModel<T>
    {
        public int TotalItemCount { get; set; }
        public int Count { get; set; }
        public int PageCount { get; set; }
        public int CurrentPage { get; set; }
        public T[] Data { get; set; }
    }
}
