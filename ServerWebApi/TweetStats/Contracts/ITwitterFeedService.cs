using TwitterSharp.Response.RTweet;

namespace ServerWebApi.TweetStats.Contracts
{
    public interface ITwitterFeedService
    {
        Task StartFeedAsync(string bearerToken, CancellationToken stoppingToken, Func<Tweet, Task> NotifyNewTweet);
    }
}
