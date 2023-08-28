using AutoMapper;
using Domain;
using Infrastructure;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("SQLConnection")));
builder.Services.AddSingleton(new MapperConfiguration(c =>
{
    c.AddProfile<AutoMapperProfile>();
}).CreateMapper());
#region Authorization
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie(options =>
{
    options.LoginPath = "/SignIn";
    options.LogoutPath = "/SignOut";
});
#endregion
#region Compression & Caching

builder.Services.Configure<GzipCompressionProviderOptions>(options => options.Level = System.IO.Compression.CompressionLevel.Fastest);

builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
    options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[]
    {
                "text/xml",
                "image/svg",
                "image/webp",
                "image/xml",
                "image/jpg",
                "image/x-icon",
                "image/png",
                "application/xml",
                "application/atom",
                "font/woff2",
                });
    options.Providers.Add<GzipCompressionProvider>();
});


builder.Services.AddResponseCaching();
builder.Services.AddMemoryCache();
builder.Services.AddDistributedMemoryCache();
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseResponseCompression();//ResponseCompression GZip

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
