using Core.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Core.Common
{
    [DataContract]
    public class PagedList<T> : IPagedList<T>
    {
        public PagedList()
        {
        }

        public PagedList(IEnumerable<T> source, int currentPage, int pageSize)
            : this(source == null ? new List<T>().AsQueryable() : source.AsQueryable(), currentPage, pageSize)
        {
        }

        private PagedList(IQueryable<T> source, int currentPage, int pageSize)
        {
            TotalItemCount = source.Count();
            CurrentPage = currentPage < 1 ? 1 : currentPage;

            if(TotalItemCount <= 0)
                return;

            Data = source.Skip((currentPage - 1) * pageSize).Take(pageSize).ToArray();
            PageCount = (int)Math.Ceiling(TotalItemCount / (decimal)pageSize);

            Count = Data.Length;
        }

        public PagedList(IEnumerable<T> source, int currentPage, int pageCount, int totalItemCount)
        {
            Count = source.Count();

            // add items to internal list
            if (Count <= 0) 
                return;
            
            TotalItemCount = totalItemCount;
            PageCount = pageCount;
            CurrentPage = currentPage;
            Data = source.ToArray();
        }

        public IPagedList<TResult> TransformData<TResult>(Func<IEnumerable<T>, IEnumerable<TResult>> transform)
        {
            var transformedData = transform(this.Data);

            return new PagedList<TResult>(transformedData, this.CurrentPage, this.PageCount, this.TotalItemCount);
        }

        T[] _Data;

        [DataMember(Name = "data")]
        public T[] Data
        {
            get {
                if (_Data == null)
                    _Data = new T[0];

                return _Data;
            }
            set {
                _Data = value;
            }
        }

        [DataMember(Name = "totalItemCount")]
        public int TotalItemCount { get; set; }

        [DataMember(Name = "currentPage")]
        public int CurrentPage { get; set; }

        [DataMember(Name = "count")]
        public int Count { get; set; }

        [DataMember(Name = "pageCount")]
        public int PageCount { get; set; }
    }
}
