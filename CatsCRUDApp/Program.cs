using BLL.Entities;
using BLL.Interfaces;
using BLL.Interfaces.Cache;
using BLL.Mediator;
using BLL.Mediator.Components;
using BLL.Services;
using CatsCRUDApp;
using CatsCRUDApp.Models;
using CatsCRUDApp.Validators;
using DAL;
using DAL.CacheAllocation;
using DAL.CacheAllocation.Cosumers;
using DAL.CacheAllocation.Producers;
using DAL.Config;
using DAL.EF;
using DAL.Finders;
using DAL.MongoDb;
using DAL.Repositories;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

//TODO:
// onconfigureMongo/Redis
// config file to Redis/Mongo

//listneres problems +
// implement automapper from streamcat to cat +
// implement redisstream name "telemetry" in configure +


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Host.ConfigureAppConfiguration(config =>
{
    var prefis = "SAMPLEAPI_";
    config.AddEnvironmentVariables(prefis);
});

builder.Services.Configure<MongoConfig>(builder.Configuration.GetSection(nameof(MongoConfig)));
builder.Services.Configure<RedisConfig>(builder.Configuration.GetSection(nameof(RedisConfig)));

builder.Services.AddSingleton<MongoConfig>(sp => sp.GetRequiredService<IOptions<MongoConfig>>().Value);

builder.Services.AddSingleton<RedisConfig>(sp => sp.GetRequiredService<IOptions<RedisConfig>>().Value);

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
builder.Services.AddSingleton<IChannelContext<CatStreamModel>, ChannelContext>();

builder.Services.AddSingleton<IChannelProducer<CatStreamModel>, ChannelProducer>();
builder.Services.AddSingleton<IChannelConsumer<CatStreamModel>, ChannelConsumer>();
builder.Services.AddSingleton<IRedisProducer, RedisProducer>();
builder.Services.AddSingleton<IRedisConsumer, RedisConsumer>();

builder.Services.AddScoped<CreateComponent>();
builder.Services.AddScoped<UpdateComponent>();
builder.Services.AddScoped<DeleteComponent>();
builder.Services.AddScoped<GetComponent>();
builder.Services.AddScoped<IMediator, ConcreteMediator>();

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

var mongoConfig = app.Services.GetService(typeof(MongoConfig)) as MongoConfig;
var redisConfig = app.Services.GetService(typeof(RedisConfig)) as RedisConfig;

var mongoIp = builder.Configuration.GetSection("SAMPLEAPI_ConnectionStrings_Mongo").Value;
var mongoPort = builder.Configuration.GetSection("SAMPLEAPI_Port_Mongo").Value;
var redisIp = builder.Configuration.GetSection("SAMPLEAPI_ConnectionStrings_Redis").Value;
var redisPort = builder.Configuration.GetSection("SAMPLEAPI_Port_Redis").Value;

if (!string.IsNullOrEmpty(mongoIp))
{
    mongoConfig.Ip = mongoIp;
}

if (!string.IsNullOrEmpty(redisIp))
{
    redisConfig.Ip = redisIp;
}

if (!string.IsNullOrEmpty(mongoPort))
{
    mongoConfig.Port = int.Parse(mongoPort);
}

if (!string.IsNullOrEmpty(redisPort))
{
    redisConfig.Port = int.Parse(redisPort);
}

Console.WriteLine(redisConfig.Ip);
Console.WriteLine(redisConfig.Port);
Console.WriteLine(mongoConfig.Ip);
Console.WriteLine(mongoConfig.Port);


var cache = app.Services.GetService(typeof(ICache<Cat>)) as Cache;

cache.ListenRedisTask();
cache.ListenChannelTask();

app.Run();