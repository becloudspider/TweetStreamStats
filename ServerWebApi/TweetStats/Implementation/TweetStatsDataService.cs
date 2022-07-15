using ServerWebApi.TweetStats.Contracts;

namespace ServerWebApi.TweetStats.Implementation
{
    public class TweetStatsDataService : ITweetStatsWriter, ITweetStatsReader
    {
        private long _tweetCount = 0;
        private Dictionary<string, int> _tagDictionary = new Dictionary<string, int>();

        public async Task SaveTweetCountAsync(int tweetCount)
        {
            _tweetCount += tweetCount;
            await Task.CompletedTask;
        }

        public async Task SaveTweetTagsAsync(IEnumerable<string> tags)
        {
            foreach (var tag in tags)
            {
                if (_tagDictionary.ContainsKey(tag))
                {
                    _tagDictionary[tag] += 1;
                }
                else
                {
                    _tagDictionary.Add(tag, 1);
                }
            }

            await Task.CompletedTask;
        }

        public async Task<TweetStat> GetStatsAsync(int topTagCount)
        {
            var topTags = _tagDictionary.OrderByDescending(t => t.Value).Take(topTagCount).Select(t => t.Key);
            return await Task.FromResult(new TweetStat(_tweetCount, topTags));
        }
    }
}
