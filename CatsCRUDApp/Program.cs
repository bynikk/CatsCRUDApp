using BLL.Entities;
using BLL.Interfaces;
using BLL.Services;
using CatsCRUDApp;
using CatsCRUDApp.Models;
using CatsCRUDApp.Validators;
using DAL;
using DAL.CacheAllocation;
using DAL.EF;
using DAL.Finders;
using DAL.MongoDb;
using DAL.Repositories;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddScoped<ICatService, CatService>();
builder.Services.AddScoped<IFinder<Cat>, CatFinderCache>();
builder.Services.AddScoped<IRepository<Cat>, CatRepositoryCache>();

builder.Services.AddScoped<IDogService, DogService>();
builder.Services.AddScoped<IFinder<Dog>, DogFinder>();
builder.Services.AddScoped<IRepository<Dog>, DogRepository>();

builder.Services.AddScoped<IValidator<Cat>, CatValidator>();
builder.Services.AddScoped<IValidator<CatViewModel>, CatViewModelValidator>();

builder.Services.AddScoped<IRedisConfiguration, RedisConfiguration>();

builder.Services.AddSingleton<ICache<Cat>, Cache>();

builder.Services.AddAutoMapper(typeof(OrganizationProfile));

builder.Services.AddScoped<IPetsContext, PetsContext>();


var optionsBuilder = new DbContextOptionsBuilder<CatDbContext>();

var options = optionsBuilder
        .UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=CatsCRUDAppDb;Trusted_Connection=True;MultipleActiveResultSets=true")
        .Options;
//builder.Services.AddScoped<CatDbContext>((IServiceProvider provider) => new CatDbContext(options));

builder.Services.AddControllers();

builder.Services.AddDbContext<CatDbContext>(options => options.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=CatsCRUDAppDb;Trusted_Connection=True;MultipleActiveResultSets=true"), ServiceLifetime.Scoped);
builder.Services.AddScoped<DbSet<Cat>>((IServiceProvider provider) => provider.GetRequiredService<CatDbContext>().Cats);


builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddMvc()
    .AddFluentValidation(fv => fv.ImplicitlyValidateRootCollectionElements = true);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

var tokenSource = new CancellationTokenSource();
var token = tokenSource.Token;
var muxer = ConnectionMultiplexer.Connect("localhost:6379");
var db = muxer.GetDatabase();

const string streamName = "telemetry";
const string groupName = "avg";

if (!(await db.KeyExistsAsync(streamName)) || (await db.StreamGroupInfoAsync(streamName)).All(x => x.Name != groupName))
{
    await db.StreamCreateConsumerGroupAsync(streamName, groupName, "0-0", true);
}

//var producerTask = Task.Run(async () =>
//{
//    var random = new Random();
//    while (!token.IsCancellationRequested)
//    {
//        await db.StreamAddAsync(streamName,
//            new NameValueEntry[]
//                {new("temp", random.Next(50, 65)), new NameValueEntry("time", DateTimeOffset.Now.ToUnixTimeSeconds())});
//        await Task.Delay(2000);
//    }
//});

Dictionary<string, string> ParseResult(StreamEntry entry) => entry.Values.ToDictionary(x => x.Name.ToString(), x => x.Value.ToString());

var readTask = Task.Run(async () =>
{
    string buff = string.Empty;

    while (!token.IsCancellationRequested)
    {
        var result = await db.StreamRangeAsync(streamName, "-", "+", 1, Order.Descending);

        if (result.Any())
        {
            //var a = db.StreamInfo(streamName).LastEntry;

            //var sb = new StringBuilder();
            //foreach (var value in a.Values)
            //{
            //    sb.Append(value.Value);
            //}

            // При изменении сразу более одного элемента остаётся только последний.
            var dict = ParseResult(result.Last());
            var sb = new StringBuilder();
            foreach (var key in dict.Keys)
            {
                sb.Append(dict[key]);
            }

            if (!string.Equals(buff, sb.ToString(), StringComparison.InvariantCultureIgnoreCase))
            {
                Console.WriteLine(buff = sb.ToString());
            }
        }

        await Task.Delay(10000);
    }
}); 

app.Run();