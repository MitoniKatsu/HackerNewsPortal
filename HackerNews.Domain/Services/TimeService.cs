using HackerNews.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackerNews.Domain.Services
{
    public class TimeService : ITimeService
    {
        public static DateTime NowUTC()
        {
            return DateTime.UtcNow;
        }
    }
}
