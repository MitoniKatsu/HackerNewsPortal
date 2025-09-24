using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackerNews.Domain.Models
{
    public abstract class PagedRequestBase
    {
        private int _pageNumber = 1;
        private int _pageSize = 10;

        public int PageNumber
        {
            get => _pageNumber;
            set => _pageNumber = value > 0 ? value : _pageNumber;
        }
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > 0 ? value : _pageSize;
        }
    }
}
