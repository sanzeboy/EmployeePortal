using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeePortal.Application
{
    public class PagedResult<T>
      where T : class
    {
        public IList<T> Results { get; set; }

        public int CurrentPage { get; set; }
        public int PageCount { get; set; }
        public int PageSize { get; set; }
        public int RowCount { get; set; }

        public int FirstRowOnPage
        {

            get { return (CurrentPage - 1) * PageSize + 1; }
        }
        public int LastRowOnPage
        {
            get { return Math.Min(CurrentPage * PageSize, RowCount); }
        }
        public bool HasPreviousPage
        {
            get
            {
                return (CurrentPage > 1);
            }
        }

        public bool HasNextPage
        {
            get
            {
                return (CurrentPage < PageCount);
            }
        }

        public PagedResult()
        {
            Results = new List<T>();
        }
    }
}
