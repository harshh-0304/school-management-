// Program.cs located in your schoolmanagement project root

using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using schoolmanagement.Data;
using schoolmanagement.Models.Auth;
using Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore; // Added for development error page (UseMigrationsEndPoint)

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// This line registers MVC controllers and views
builder.Services.AddControllersWithViews();
// This is needed for Identity UI (Razor Pages for Login, Register, etc.)
builder.Services.AddRazorPages();

// Configure Entity Framework Core DbContext
builder.Services.AddDbContext<SchoolDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.")));

// Add ASP.NET Core Identity services
builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    // For local development, setting RequireConfirmedAccount to false is convenient.
    // For production, you typically want to set this to true for email confirmation.
    options.SignIn.RequireConfirmedAccount = false;
}).AddEntityFrameworkStores<SchoolDbContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
// These lines set up how requests are processed (e.g., error handling, routing)
if (app.Environment.IsDevelopment()) // Changed from !app.Environment.IsDevelopment() to enable dev tools
{
    // Shows database migration errors during development
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection(); // Redirects HTTP requests to HTTPS
app.UseStaticFiles();      // Serves static files like CSS, JavaScript, images

app.UseRouting();          // Matches incoming requests to endpoints (like controller actions)

// IMPORTANT: Authentication middleware must come before Authorization middleware
app.UseAuthentication();   // Required for Identity to work (identifies who the user is)
app.UseAuthorization();    // Handles user authorization (what the identified user can do)

// Maps Razor Pages (essential for Identity area pages like /Identity/Account/Login)
app.MapRazorPages();

// Maps default routing for MVC controllers (e.g., /Home/Index, /Students/Index)
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run(); // Starts the application
