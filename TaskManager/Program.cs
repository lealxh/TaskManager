
using Microsoft.EntityFrameworkCore;
using TaskManager.Interfaces;
using TaskManager.Models;

var builder = WebApplication.CreateBuilder(args);

var Configuration = builder.Configuration;


// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IDbContextFactory, DbContextFactory>();
builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

//builder.Services.AddScoped<IDbContextFactory, DbContextFactory>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Tasks/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Tasks}/{action=Index}/{id?}");

app.Run();
