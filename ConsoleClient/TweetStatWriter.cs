using Microsoft.Extensions.Hosting;

internal class TweetStatWriter : IHostedService
{
    HttpClient _client;
    readonly string _url;
    public TweetStatWriter()
    {
        _url = "https://localhost:7200/TweetStats";
        _client = new HttpClient();
    }
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("Hello, This is a Tweet Stat Client, please stay tune for interesting Tweeter sample stream stats.");

        while (!cancellationToken.IsCancellationRequested)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, _url);
            var response = await _client.SendAsync(request, cancellationToken);
            var responseText = await response.Content.ReadAsStringAsync();

            Console.WriteLine(responseText);
            
            await Task.Delay(1000);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _client.Dispose();
        return Task.CompletedTask;
    }
}