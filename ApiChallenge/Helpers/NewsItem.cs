using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiChallenge.Helpers
{
    public class NewsItem
    {
        public NewsItem()
        {
            this.kidsItems = new List<NewsItem>();
        }

        public int id { get; set; }
        public string title { get; set; }
        public string by { get; set; }
        public int score { get; set; }
        public string url { get; set; }

        string _text;
        public string text
        {
            get { return System.Web.HttpUtility.HtmlDecode(_text); }
            set { _text = value; }
        }
        public int time { get; set; }

        public int descendants { get; set; }
        public string type { get; set; }

        public List<int> kids { get; set; }
        public List<NewsItem> kidsItems { get; set; }
    }
}
