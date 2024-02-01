using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Policy;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60);
});
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.Cookie.Name = "Cookie";
    options.Cookie.SameSite = SameSiteMode.None;
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    options.LoginPath = "/Login/Login";
    options.LogoutPath = "/Login/Logout";
    options.AccessDeniedPath = "/Account/Forbidden";
});
var app = builder.Build();

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
app.UseSession();
app.UseAuthorization();



//app.UseEndpoints(endpoints =>
//{
//    app.MapAreaControllerRoute(
//                name: "Master",
//                areaName: "Master",
//                pattern: "{area:exists}/{controller=CodesMaster}/{action=CodesMaster}/{id1?}/{id2?}/{id3?}/{id4?}/{id5?}");
//    app.MapAreaControllerRoute(
//                name: "Transaction",
//                areaName: "Transaction",
//                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id1?}/{id2?}/{id3?}/{id4?}/{id5?}");

//    app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Login}/{action=Login}/{id?}");
//});
app.UseEndpoints(endpoints =>
{

    endpoints.MapAreaControllerRoute(
                name: "Master",
                areaName: "Master",
               pattern: "{area=Master}/{controller=Home}/{action=Index}/{id1?}/{id2?}/{id3?}/{id4?}/{id5?}");
    endpoints.MapAreaControllerRoute(
                name: "Transaction",
                areaName: "Transaction",
               pattern: "{area=Transaction}/{controller=Home}/{action=Index}/{id1?}/{id2?}/{id3?}/{id4?}/{id5?}");
    endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Login}/{action=Login}/{id1?}"
                );




}
);


app.Run();
