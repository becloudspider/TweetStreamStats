namespace ServerWebApi.TweetStats.Contracts
{
    public interface ITweetStatsProcessingService
    {
        Task ProcessAsync(string bearerToken, CancellationToken stoppingToken);
    }
}
