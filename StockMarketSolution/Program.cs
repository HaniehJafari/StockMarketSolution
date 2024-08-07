using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using Services;
using StockMarketSolution;
using RepositoryContracts;
using Repositories;

var builder = WebApplication.CreateBuilder(args);

//Services
builder.Services.AddControllersWithViews();
builder.Services.Configure<TradingOptions>(builder.Configuration.GetSection("TradingOptions"));

builder.Services.AddTransient<IStocksService, StocksService>();
builder.Services.AddTransient<IFinnhubService, FinnhubService>();
builder.Services.AddTransient<IFinnhubRepository, FinnhubRepository>();
builder.Services.AddTransient<IStocksRepository, StocksRepository>();




builder.Services.AddDbContext<StockMarketDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddHttpClient();

var app = builder.Build();

if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

Rotativa.AspNetCore.RotativaConfiguration.Setup("wwwroot", wkhtmltopdfRelativePath:"Rotativa");


app.UseStaticFiles();
app.UseRouting();
app.MapControllers();

app.Run();


