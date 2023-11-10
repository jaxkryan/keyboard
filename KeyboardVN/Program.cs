using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using KeyboardVN.Models;
using Microsoft.Extensions.Options;
using KeyboardVN.Areas.Seller.Controllers;
using KeyboardVN.Util.EmailSender;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("KeyboardVNContextConnection") ?? throw new InvalidOperationException("Connection string 'KeyboardVNContextConnection' not found.");
var mailsettings = builder.Configuration.GetSection("MailSettings");

builder.Services.AddHttpContextAccessor();

builder.Services.ConfigureApplicationCookie(options => {
    // options.Cookie.HttpOnly = true;
    // options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
    options.LoginPath = $"/login/";
    options.LogoutPath = $"/logout/";
    options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
});

builder.Services.AddOptions();
builder.Services.Configure<MailSettings>(mailsettings);
builder.Services.AddTransient<ISendMailService, SendMailService>();

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
});

builder.Services.AddDbContext<KeyboardVNContext>(options =>
{
    options.UseSqlServer(connectionString);
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

});

builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<Role>()
    .AddEntityFrameworkStores<KeyboardVNContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add services to send mail 
builder.Services.AddScoped<ISendMailService, SendMailService>();
builder.Services.AddScoped<ManageOrderController>(); // Add services to controller ManageOrderController

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseSession();

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); ;

app.UseAuthorization();

app.MapControllerRoute(
      name: "area",
      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
      );

app.MapAreaControllerRoute(
   name: "default",
   areaName: "Guest",
   pattern: "{controller=Home}/{action=Index}/{id?}"
   );

app.MapRazorPages();

app.Run();
