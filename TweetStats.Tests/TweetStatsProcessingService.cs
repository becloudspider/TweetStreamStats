using Microsoft.Extensions.Logging;
using Moq;
using ServerWebApi.TweetStats.Contracts;
using ServerWebApi.TweetStats.Implementation;
using TwitterSharp.Response.RTweet;

namespace TweetStats.Tests
{
    [TestClass]
    public class TweetStatsProcessingServiceTests
    {
        ITweetStatsProcessingService _processingService;
        StubTwitterFeedService _twitterFeedService;
        Mock<ITweetStatsWriter> _mockWriter;
        Mock<ILogger<TweetStatsProcessingService>> _mockLogger;
        int sampleTweetCount = 0;

        [TestInitialize]
        public void Setup()
        {
            sampleTweetCount = 5000;
            _twitterFeedService = new StubTwitterFeedService(TestData.GetDefault(sampleTweetCount));
            _mockWriter = new Mock<ITweetStatsWriter>();
            _mockLogger = new Mock<ILogger<TweetStatsProcessingService>>();

            _processingService = new TweetStatsProcessingService(_twitterFeedService, _mockWriter.Object, _mockLogger.Object);
        }

        [TestMethod]
        public async Task VerifyMockCallsAsync()
        {
            //mock calls called verification
            await _processingService.ProcessAsync(string.Empty, new CancellationToken());

            _mockWriter.Verify(x => x.SaveTweetCountAsync(It.Is<int>(x => x == 1)), Times.Exactly(sampleTweetCount));
            _mockWriter.Verify(x => x.SaveTweetTagsAsync(It.Is<IEnumerable<string>>(x => x.Count() == 0)), Times.Exactly(sampleTweetCount));
        }
    }

    internal class StubTwitterFeedService : ITwitterFeedService
    {
        public StubTwitterFeedService(List<Tweet> tweets)
        {
            SampleTweets = tweets;
        }
        public List<Tweet> SampleTweets { get; }

        public async Task StartFeedAsync(string bearerToken, CancellationToken stoppingToken, Func<Tweet, Task> NotifyNewTweet)
        {
            foreach (var tweet in SampleTweets)
            {
                await NotifyNewTweet(tweet);
            }
        }
    }

    internal class TestData
    {
        public static List<Tweet> GetDefault(int tweetCount)
        {
            return new Bogus.Faker<Tweet>().Generate(tweetCount);
        }
    }
}
