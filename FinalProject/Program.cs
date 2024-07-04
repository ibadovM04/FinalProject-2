using FinalProject.Data;
using FinalProject.Interfaces;
using FinalProject.Notifications.Slack;
using FinalProject.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMemoryCache();

builder.Services.AddSingleton<SlackService>();
//builder.Services.AddTransient<QrCodeService>();
builder.Services.AddTransient<IProductManager, ProductManager>();
builder.Services.AddTransient<IUserManager, UserManager>();


builder.Services.AddHttpContextAccessor();


builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration["Database:Connection"]);
});

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(50);
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, config =>
    {
        config.LoginPath = $"/Account/Login";

        config.ExpireTimeSpan = TimeSpan.FromDays(1);

        config.SlidingExpiration = false;

        config.Cookie.IsEssential = true;
    })
    .AddCookie("AdminCookies", config =>
    {
        config.LoginPath = $"/Admin/Account/Login";

        config.ExpireTimeSpan = TimeSpan.FromDays(1);

        config.SlidingExpiration = false;

        config.Cookie.IsEssential = true;
    });
builder.Services.AddRazorPages();
//builder.Services.AddAuthentication("CookieAuth")
 
//    .AddCookie("CookieAuth", options =>
//    {
//        options.Cookie.Name = "MyApp.Cookie";
//        options.LoginPath = "/Account/Logout";
//    });



builder.Services.AddControllersWithViews();

var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.UseSession();
app.UseHttpsRedirection();
app.UseRouting();
app.UseStaticFiles();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute("admin", "{area:exists}/{controller=Home}/{action=Index}");

app.MapControllerRoute("default", "{controller=Home}/{action=Index}");

app.Run();
