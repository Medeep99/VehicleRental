using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using VehicleRentalProject.Repositories;
using VehicleRentalProject.Repositories.Implementation;
using VehicleRentalProject.Repositories.Infrastructure;
using VehicleRentalProject.Web.CustomMiddleWare;
using VehicleRentalProject.Web.Mapper;
using Microsoft.AspNetCore.Identity;
using VehicleRentalProject.Models;
using VehicleRentalProject.Repositories.DataSeeding;
using VehicleRentalProject.Web.Utility;
using Stripe;
using Microsoft.AspNetCore.Identity.UI.Services;

var builder = WebApplication.CreateBuilder(args);

// Configure dbcontext with connectionstring
builder.Services.AddDbContext<CarContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddDefaultTokenProviders()
    .AddEntityFrameworkStores<CarContext>();

builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("PaymentSettings"));

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IVehicleRepository, VehicleRepository>();
builder.Services.AddScoped<IDbInitializer, DbInitializer>();
builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IOrderHeaderService, OrderHeaderService>();
builder.Services.AddScoped<IOrderDetailService, OrderDetailService>();

var config = new AutoMapper.MapperConfiguration(cfg =>
{
    cfg.AddProfile(new VehicleProfile(builder.Environment));
});
var mapper = config.CreateMapper();
builder.Services.AddSingleton(mapper);

builder.Services.AddSession(options =>
{
    options.Cookie.Name = "VehicleProjectCookie"; // Set a unique name for the session cookie
    options.IdleTimeout = TimeSpan.FromMinutes(1); // Set the session timeout duration
    options.Cookie.HttpOnly = true; // Ensures the session cookie is only accessible over HTTP
});
builder.Services.AddRazorPages();
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = $"/Identity/Account/Login";
    options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
    options.LogoutPath = $"/Identity/Account/Logout";
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// app.UseMiddleware<ExceptionHandlerMiddleware>();
app.UseStaticFiles(); // Example of static files middleware
DataSeeding();
app.UseSession();
app.UseRouting();

StripeConfiguration.ApiKey = builder.Configuration.GetSection("PaymentSettings:SecretKey").Get<string>();

app.UseAuthentication();
app.UseAuthorization();

#pragma warning disable ASP0014
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "areas",
        pattern: "{area:exists}/{controller=vehicles}/{action=Index}/{id?}");

    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});
#pragma warning restore ASP0014

app.MapRazorPages();
app.Run();

void DataSeeding()
{
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<CarContext>();
        if (dbContext.Database.GetPendingMigrations().Any())
        {
            dbContext.Database.Migrate();
        }
        var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
        dbInitializer.Initialize();
    }
}
