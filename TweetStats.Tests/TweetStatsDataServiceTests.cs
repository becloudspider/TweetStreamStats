using ServerWebApi.TweetStats.Contracts;
using ServerWebApi.TweetStats.Implementation;

namespace TweetStats.Tests
{
    [TestClass]
    public class TweetStatsDataServiceTests
    {
        ITweetStatsReader _reader;
        ITweetStatsWriter _writer;

        [TestInitialize]
        public void Setup()
        {
            var dataService = new TweetStatsDataService();
            _reader = dataService;
            _writer = dataService;
        }

        [TestMethod]
        public async Task EmptyReadAsync()
        {
            //empty read
            var stats = await _reader.GetStatsAsync(10);

            Assert.AreEqual(0, stats.TweetCount);
            Assert.AreEqual(0, stats.TopTags.Count());
        }

        [TestMethod]
        public async Task DataIntegrityAsync()
        {
            //write and read verify
            var tagsListOne = new List<string> { "Tag1", "Tag2", "Tag3" };
            await _writer.SaveTweetCountAsync(20);
            await _writer.SaveTweetTagsAsync(tagsListOne);

            var stats = await _reader.GetStatsAsync(10);

            Assert.AreEqual(20, stats.TweetCount);
            Assert.AreEqual(3, stats.TopTags.Count());

            var tagsListTwo = new List<string> { "Tag1", "Tag2", "Tag3", "Tag3", "Tag3", "Tag3", "Tag3" };
            await _writer.SaveTweetCountAsync(30);
            await _writer.SaveTweetTagsAsync(tagsListTwo);

            stats = await _reader.GetStatsAsync(1);

            Assert.AreEqual(50, stats.TweetCount);
            Assert.AreEqual(1, stats.TopTags.Count());
            Assert.AreEqual("Tag3", stats.TopTags.First());
        }
    }
}