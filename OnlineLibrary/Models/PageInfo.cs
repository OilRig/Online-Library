using System;
using System.Collections.Generic;

namespace OnlineLibrary.Models
{
    public class PageInfo
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages
        {
            get { return (int)Math.Ceiling((decimal)TotalItems / PageSize); }
        }
    }
    public class AllBooksViewModel
    {
        public IEnumerable<BookViewModel> Books { get; set; }
        public PageInfo PageInfo { get; set; }
    }
}