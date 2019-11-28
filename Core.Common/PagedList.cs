using Core.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Core.Common
{
    [DataContract]
    public class PagedList<T> : IPagedList<T>
    {
        public PagedList()
        {
        }

        public PagedList(IEnumerable<T> source, int startRowIndex, int pageSize)
            : this(source == null ? new List<T>().AsQueryable() : source.AsQueryable(), startRowIndex, pageSize)
        {
        }

        private PagedList(IQueryable<T> source, int startRowIndex, int pageSize)
        {
            _TotalItemCount = source.Count();

            // add items to internal list
            if (_TotalItemCount > 0)
            {
                if (startRowIndex == -1)
                {
                    Data = source.Take(pageSize).ToArray();
                    PageCount = 1;
                }
                else
                {
                    Data = source.Skip(startRowIndex).Take(pageSize).ToArray();
                    PageCount = (int)Math.Ceiling(TotalItemCount / (decimal)pageSize);
                }

                Count = Data.Length;
            }
        }

        public PagedList(IEnumerable<T> source, int currentPage, int pageCount, int totalItemCount)
        {
            _Count = source.Count();

            // add items to internal list
            if (_Count > 0)
            {
                TotalItemCount = totalItemCount;
                PageCount = pageCount;
                CurrentPage = currentPage;
                Data = source.ToArray();
            }
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

        int _TotalItemCount;

        [DataMember(Name = "totalitemcount")]
        public int TotalItemCount
        {
            get {
                return _TotalItemCount;
            }
            set {
                _TotalItemCount = value;
            }
        }

        int _CurrentPage;

        [DataMember(Name = "currentpage")]
        public int CurrentPage
        {
            get {
                return _CurrentPage;
            }
            set {
                _CurrentPage = value;
            }
        }

        int _Count;

        [DataMember(Name = "count")]
        public int Count
        {
            get {
                return _Count;
            }
            set {
                _Count = value;
            }
        }

        int _PageCount;

        [DataMember(Name = "pagecount")]
        public int PageCount
        {
            get { return _PageCount; }
            set { _PageCount = value; }
        }
    }
}
