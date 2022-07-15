namespace ServerWebApi.TweetStats.Contracts
{
    public interface ITweetStatsWriter
    {
        Task SaveTweetCountAsync(int tweetCount);
        Task SaveTweetTagsAsync(IEnumerable<string> tags);
    }

    public record TweetStat(long TweetCount, IEnumerable<string> TopTags);
    public interface ITweetStatsReader
    {
        Task<TweetStat> GetStatsAsync(int topTagCount);
    }
}