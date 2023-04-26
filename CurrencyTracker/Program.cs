using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using CurrencyTracker.Data;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<CurrencyTrackerContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CurrencyTrackerContext") ?? throw new InvalidOperationException("Connection string 'CurrencyTrackerContext' not found.")));

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
    endpoints.MapRazorPages();
});

app.Run();
