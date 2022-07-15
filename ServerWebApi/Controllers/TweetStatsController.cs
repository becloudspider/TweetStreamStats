using Microsoft.AspNetCore.Mvc;
using ServerWebApi.TweetStats.Contracts;

namespace ServerWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TweetStatsController : ControllerBase
    {
        private readonly ITweetStatsReader _tweetStatsReader;

        public TweetStatsController(ITweetStatsReader tweetStatsReader)
        {
            _tweetStatsReader = tweetStatsReader;
        }

        [HttpGet]
        public async Task<TweetStat> Get()
        {
            return await _tweetStatsReader.GetStatsAsync(10);
        }
    }
}