using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiChallenge.Helpers
{
    public class NewsApiItems
    {
        public int TotalItemsProcessed { get; set; }
        public int PageNumber { get; set; }
        public int LastStoryNumber { get; set; }
        public string message { get; set; }
        public List<NewsItem> Stories { get; set; }
    }
}
