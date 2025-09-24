using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackerNews.Domain.Models.DTO
{
    public class PagedResponseDto<T>
    {
        public int Total { get; set; }
        public int PageNumber { get; set; }
        public ICollection<T> Page { get; set; } = [];
    }
}
