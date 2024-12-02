using EmployeePortal.Application;
using EmployeePortal.Application.Infrastructures;
using EmployeePortal.Application.Services.AppUsers;
using EmployeePortal.Infrastructure;
using EmployeePortal.UI;
using EmployeePortal.UI.Middlewares;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using static Org.BouncyCastle.Math.EC.ECCurve;

var builder = WebApplication.CreateBuilder(args);



// Add httpContext
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddHttpContextAccessor();

builder.Services.ConfigureAppSettings(builder.Configuration);

// Add Application Dependencies
builder.Services.AddApplication();



// Add Infastructure Dependencies
builder.Services.AddInfrastructure(builder.Configuration);

// Add services to the container.
builder.Services.AddSingleton<ICurrentUserService, CurrentUserService>();

builder.Services.AddHttpClient();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(x =>
    {
        x.LoginPath = "/Account/Login";
        x.LogoutPath = "/Account/Logout";
    });
builder.Services.AddAuthorization();
builder.Services.AddControllersWithViews();

var app = builder.Build();
app.UseMiddleware<CustomExceptionMiddleware>();
await app.MigrateDatabase();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

var imageDirectoryPath = Path.Combine(Directory.GetCurrentDirectory(), "Images");
if (!Directory.Exists(imageDirectoryPath))
    Directory.CreateDirectory(imageDirectoryPath);

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(imageDirectoryPath)
                ,
    RequestPath = "/Images"
});

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
