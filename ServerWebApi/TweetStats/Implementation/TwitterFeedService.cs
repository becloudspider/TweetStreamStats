using ServerWebApi.TweetStats.Contracts;
using TwitterSharp.Client;
using TwitterSharp.Request.AdvancedSearch;
using TwitterSharp.Request.Option;
using TwitterSharp.Response.RTweet;

namespace ServerWebApi.TweetStats.Implementation
{
    public class TwitterFeedService : ITwitterFeedService
    {
        public async Task StartFeedAsync(string bearerToken, CancellationToken stoppingToken, Func<Tweet, Task> NotifyNewTweet)
        {
            using (var client = new TwitterClient(bearerToken))
            {
                var options = new TweetSearchOptions()
                {
                    TweetOptions = new TweetOption[] { TweetOption.Entities }
                };

                while (!stoppingToken.IsCancellationRequested)
                {
                    await foreach (var tweet in client.NextTweetSampleStreamAsync(options))
                    {
                        await NotifyNewTweet(tweet);
                    }
                }

                client.CancelTweetStream();
            }
        }
    }
}
