using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using CMS.Models;

var builder = WebApplication.CreateBuilder(args); // Chỉ gọi 1 lần

// Cấu hình kết nối database
builder.Services.AddDbContext<CinemaDbContext>(options =>
    options.UseSqlServer("Server=DESKTOP-UQHR23R;Database=CinemaManagement2;User Id=sa;Password=123;TrustServerCertificate=True"));

// Cấu hình MVC + View Compilation
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

// Cấu hình Session & HTTP Context
builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();

// Cấu hình JSON
builder.Services.AddControllers()
    .AddNewtonsoftJson()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    });

builder.Logging.ClearProviders();
builder.Logging.AddConsole();


// Đọc cấu hình từ appsettings.json
IConfigurationRoot cf = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

// **Không cần gọi lại AddDbContext lần thứ 2!**

// Build ứng dụng
var app = builder.Build();

// Middleware mặc định
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();   
app.UseRouting();

// Kích hoạt Session
app.UseSession();

app.UseAuthorization();

// Cấu hình route mặc định
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
