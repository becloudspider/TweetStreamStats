using ServerWebApi.TweetStats.Contracts;

namespace ServerWebApi.TweetStats.Implementation
{
    public class TweetStatsProcessingService : ITweetStatsProcessingService
    {
        private readonly ITwitterFeedService _twitterFeedService;
        private readonly ITweetStatsWriter _statsWriter;
        private readonly ILogger<TweetStatsProcessingService> _logger;

        public TweetStatsProcessingService(ITwitterFeedService twitterFeedService,
                                      ITweetStatsWriter tweetStatsWriter,
                                      ILogger<TweetStatsProcessingService> logger)
        {
            _twitterFeedService = twitterFeedService;
            _statsWriter = tweetStatsWriter;
            _logger = logger;
        }

        public async Task ProcessAsync(string bearerToken, CancellationToken stoppingToken)
        {
            _logger.LogInformation("TweeterStreamerService running at: {time}", DateTimeOffset.Now);
            await _twitterFeedService.StartFeedAsync(bearerToken, stoppingToken, HandleNewTweetAsync);
        }

        private async Task HandleNewTweetAsync(TwitterSharp.Response.RTweet.Tweet tweet)
        {
            const int TWEETCOUNT = 1;
            try
            {
                _logger.LogInformation($"Received tweet...");

                var tags = (tweet.Entities?.Hashtags?.Any() ?? false) ?
                            tweet.Entities.Hashtags.Select(t => t.Tag).ToList() : new List<string>();

                await WriteTweetStatsAsync(TWEETCOUNT, tags);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }

        private async Task WriteTweetStatsAsync(int tweetCount, IEnumerable<string> tags)
        {
            await _statsWriter.SaveTweetCountAsync(tweetCount);
            await _statsWriter.SaveTweetTagsAsync(tags);
        }
    }
}
