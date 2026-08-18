using System.Configuration;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// ── Bridge ConfigurationManager ← appsettings.json ─────────────────────────
// BUS/DAL dùng System.Configuration.ConfigurationManager để đọc connection string
// Cần đồng bộ từ IConfiguration sang ConfigurationManager
var connStr = builder.Configuration.GetConnectionString("QuanLyCuaHangLaptop");
if (!string.IsNullOrWhiteSpace(connStr))
{
    // Xóa entry cũ nếu có và thêm lại từ appsettings.json
    var cssSection = System.Configuration.ConfigurationManager.ConnectionStrings["QuanLyCuaHangLaptop"];
    if (cssSection == null)
    {
        // Add connection string using ASP.NET Core configuration APIs
        // Avoid mutating System.Configuration.ConnectionStrings which is read-only in this host
        builder.Configuration.AddInMemoryCollection(new[]
        {
            new System.Collections.Generic.KeyValuePair<string, string>(
                "ConnectionStrings:QuanLyCuaHangLaptop", connStr)
        });
        
    }
}

// ── Add services ─────────────────────────────────────────────────────────────
builder.Services.AddControllersWithViews();

// Session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.Name = ".BulaBula.Session";
});

// IHttpContextAccessor (dùng trong helpers)
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// ── Configure pipeline ────────────────────────────────────────────────────────
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
