using HackerNews.Domain.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackerNews.Domain.Models
{
    public class NewsSearchRequest : PagedRequestBase
    {
        public string SearchString { get; set; } = string.Empty;
    }
}
