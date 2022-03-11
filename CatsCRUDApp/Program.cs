using BLL.Entities;
using BLL.Interfaces;
using BLL.Services;
using CatsCRUDApp;
using CatsCRUDApp.Models;
using CatsCRUDApp.Validators;
using DAL.EF;
using DAL.Finders;
using DAL.Repositories;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddScoped<ICatService, CatService>();
builder.Services.AddScoped<IFinder<Cat>, CatFinder>();
builder.Services.AddScoped<IRepository<Cat>, CatRepository>();
builder.Services.AddScoped<IUnitOfWork, EFUnitOfWork>();

builder.Services.AddScoped<IValidator<Cat>, CatValidator>();
builder.Services.AddScoped<IValidator<CatViewModel>, CatViewModelValidator>();

builder.Services.AddAutoMapper(typeof(OrganizationProfile));

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

builder.Services.AddMvc().AddFluentValidation(fv => fv.ImplicitlyValidateRootCollectionElements = true);

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
