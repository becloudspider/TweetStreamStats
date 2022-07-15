using ServerWebApi.TweetStats.Contracts;

namespace ServerWebApi.TweetStats
{
    public class TweetSampleStreamStatsJob : BackgroundService
    {
        private readonly ILogger<TweetSampleStreamStatsJob> _logger;
        private readonly IConfiguration _configuration;
        private readonly ITweetStatsProcessingService _tweeterStreamer;
        public TweetSampleStreamStatsJob(ILogger<TweetSampleStreamStatsJob> logger, IConfiguration configuration, ITweetStatsProcessingService tweeterStreamer)
        {
            _logger = logger;
            _configuration = configuration;
            _tweeterStreamer = tweeterStreamer;
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var token = _configuration.GetSection("AppSettings")["TweeterBearerToken"];

            if (string.IsNullOrWhiteSpace(token))
            {
                _logger.LogError("Twitter bearer token is empty.");
                await Task.CompletedTask;
            }
            else
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    _logger.LogInformation("Starting TweeterStreamer at: {time}", DateTimeOffset.Now);
                    await _tweeterStreamer.ProcessAsync(token, stoppingToken);
                }
            }
        }
    }
}
