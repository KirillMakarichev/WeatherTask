using Microsoft.EntityFrameworkCore;
using WeatherForecast.Database;
using WeatherForecast.Services;
using WeatherForecast.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddTransient<IWeatherParser, XlsxParser>();
builder.Services.AddTransient<IArchivesRepository, ArchivesRepository>();
var connectionString = builder.Configuration.GetConnectionString("Postgres");
builder.Services.AddDbContextPool<ArchivesContext>(x => { x.UseNpgsql(connectionString); });
var app = builder.Build();
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();