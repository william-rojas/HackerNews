using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiChallenge.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApiChallenge.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HackerNewsController : ControllerBase
    {
        private enum StoryTypes
        {
            story, comment, job, poll
        };

        NewsApiService Service
        {
            get
            {
                var cfg = new NewsApiConfig(this._configuration);
                return new NewsApiService(cfg);
            }
        }

        private readonly ILogger<HackerNewsController> _logger;
        private IConfiguration _configuration { get; set; }

        public HackerNewsController(ILogger<HackerNewsController> logger, IConfiguration configuration)
        {
            _configuration = configuration;
            _logger = logger;
        }

        [HttpGet]
        [Route("GetAsyncNew")]
        public async Task<NewsApiItems> GetAsync_new(int pageNumber = 0, int storyNumber = 0, bool increment = false)
        {
            var startTime = DateTime.Now;
            var maxStoryNumber = await this.Service.GetMaxItem();

            if (storyNumber == 0 || pageNumber == 0)
            {
                storyNumber = maxStoryNumber;
                increment = true;
                pageNumber = 0;
            }
            else
                storyNumber = increment ? storyNumber - 1 : storyNumber + 1;    //do not grab the story passed

            
            var pageSize = new NewsApiConfig(this._configuration).PageSize;
            var items = new List<NewsItem>();
            var i = 0;
            var itemsFetched = 0;
            var storiesCounter = 0;
            do
            {
                var tasks = new List<Task<NewsItem>>();
                for (int p = 0; p < pageSize; p++)
                {
                    var item = this.GetDetail(storyNumber);
                    storyNumber = increment ? storyNumber - 1 : storyNumber + 1;
                    tasks.Add(item);
                }

                foreach (var task in tasks)
                {
                    var v = await task;
                    itemsFetched++;
                    try
                    {
                        if (v != null && v.type == StoryTypes.story.ToString())
                        {
                            storiesCounter++;
                            items.Add(v);
                            i++;
                        }
                    }
                    catch (Exception ex)
                    {
                        //for some reason the newer stories are somehow malformed, need to add the try catch
                        //throw;
                    }

                    
                }

            }
            while (i < pageSize);

            var diffTime = DateTime.Now.Subtract(startTime);

            return new NewsApiItems()
            {
                Stories = items.OrderByDescending(t => t.id).ToList(),
                TotalItemsProcessed = itemsFetched,
                PageNumber = pageNumber,
                //LastStoryNumber = items.Last().id,
                message = $"Note: Processed {itemsFetched} items in {diffTime.TotalSeconds} sec., {items.Count} of those items were stories"
            };
        }


        [HttpGet]
        [Route("GetAsync")]
        public async Task<IEnumerable<NewsItem>> GetAsync()
        {
            var max = await this.Service.GetMaxItem();
            var items = new List<NewsItem>();
            var i = 0;
            var itemsFetched = 0;

            while (i < 20)
            {
                var item = await this.GetDetail(max--);
                itemsFetched++;
                if (item.type == StoryTypes.story.ToString())
                {
                    items.Add(item);
                    i++;
                }
            }

            return items;
        }


        [HttpGet]
        [Route("GetDetail")]
        public async Task<NewsItem> GetDetail(int id, bool includeChildren = false)
        {
            var detail = await this.Service.GetDetails(id);

            if (includeChildren && detail.kids != null)
            {
                var tasks = new List<Task<NewsItem>>();
                foreach (var kid in detail.kids)
                {
                    var task = this.GetDetail(kid);
                    tasks.Add(task);
                }

                foreach (var task in tasks)
                {
                    var v = await task;
                    detail.kidsItems.Add(v);
                }
            }

            return detail;
        }

    }


}
