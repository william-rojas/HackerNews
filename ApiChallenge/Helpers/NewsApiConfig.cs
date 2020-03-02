using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiChallenge.Helpers
{
    public class EndPoint
    {
        public string Name { get; set; }
        public string Uri { get; set; }
    }

    public class NewsApiConfig 
    {
        public string BaseUri { get; set; }
        public string ApiKey { get; set; }
        public int PageSize { get; set; }

        public List<EndPoint> EndPoints { get; set; }

        public NewsApiConfig(IConfiguration configuration)
        {
            configuration.GetSection("NewsApi").Bind(this);
        }
    }
}
