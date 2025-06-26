// // Program.cs located in your schoolmanagement project root

// using Microsoft.EntityFrameworkCore; // Required for UseSqlServer
// using schoolmanagement.Data; // Required to access your SchoolDbContext
// using Microsoft.AspNetCore.Identity;
// using schoolmanagement.Models.Auth;


// var builder = WebApplication.CreateBuilder(args);

// // Add services to the container.
// // This line registers MVC controllers and views
// builder.Services.AddControllersWithViews();

// // Configure Entity Framework Core DbContext
// // This line tells your application to use SchoolDbContext
// // and connect to a SQL Server database using the "DefaultConnection" string
// builder.Services.AddDbContext<SchoolDbContext>(options =>
//     options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.")));

// builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<SchoolDbContext>();

// var app = builder.Build();

// // Configure the HTTP request pipeline.
// // These lines set up how requests are processed (e.g., error handling, routing)
// if (!app.Environment.IsDevelopment())
// {
//     app.UseExceptionHandler("/Home/Error");
//     // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//     app.UseHsts();
// }

// app.UseHttpsRedirection(); // Redirects HTTP requests to HTTPS
// app.UseStaticFiles();      // Serves static files like CSS, JavaScript, images

// app.UseRouting();          // Matches incoming requests to endpoints (like controller actions)

// app.UseAuthorization();    // Handles user authorization (if you add authentication later)

// // Maps default routing for MVC controllers (e.g., /Home/Index)
// app.MapControllerRoute(
//     name: "default",
//     pattern: "{controller=Home}/{action=Index}/{id?}");

// app.Run(); // Starts the application
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using schoolmanagement.Data;
using schoolmanagement.Models.Auth;

var builder = WebApplication.CreateBuilder(args);

// Add MVC controllers + Razor pages support
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages(); // 👈 This is needed for Identity pages

// Add EF Core DB context
builder.Services.AddDbContext<SchoolDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.")));

// Add Identity
builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false; // For local dev
}).AddEntityFrameworkStores<SchoolDbContext>();

var app = builder.Build();

// Error handling
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Add these two lines 👇👇 to support Identity
app.UseAuthentication(); // 👈 Required for Identity to work
app.UseAuthorization();

// Add support for Razor Pages (Login, Register etc.)
app.MapRazorPages(); // 👈 This maps Identity area pages like /Account/Login

// MVC controller routes
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
