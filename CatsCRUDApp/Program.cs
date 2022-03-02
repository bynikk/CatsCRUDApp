using BLL.Entities;
using BLL.Interfaces;
using BLL.Services;
using DAL.EF;
using DAL.Finders;
using DAL.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddTransient<ICatService, CatService>();
builder.Services.AddTransient<IFinder<Cat>, CatFinder>();
builder.Services.AddTransient<IRepository<Cat>, CatRepository>();
builder.Services.AddTransient<IUnitOfWork, EFUnitOfWork>();

builder.Services.AddControllers();

builder.Services.AddDbContext<CatDbContext>(options => options.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=CatsCRUDAppDb;Trusted_Connection=True;MultipleActiveResultSets=true"));



builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddMvc();

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

app.Run();
