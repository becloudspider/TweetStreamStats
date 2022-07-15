using ServerWebApi.TweetStats;
using ServerWebApi.TweetStats.Contracts;
using ServerWebApi.TweetStats.Implementation;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddSingleton<TweetStatsDataService>();
builder.Services.AddSingleton<ITweetStatsReader>(x => x.GetRequiredService<TweetStatsDataService>());
builder.Services.AddSingleton<ITweetStatsWriter>(x => x.GetRequiredService<TweetStatsDataService>());
builder.Services.AddSingleton<ITwitterFeedService, TwitterFeedService>();
builder.Services.AddSingleton<ITweetStatsProcessingService, TweetStatsProcessingService>();

builder.Services.AddHostedService<TweetSampleStreamStatsJob>();

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
