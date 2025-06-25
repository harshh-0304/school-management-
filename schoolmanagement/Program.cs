// Program.cs located in your schoolmanagement project root

using Microsoft.EntityFrameworkCore; // Required for UseSqlServer
using schoolmanagement.Data; // Required to access your SchoolDbContext

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// This line registers MVC controllers and views
builder.Services.AddControllersWithViews();

// Configure Entity Framework Core DbContext
// This line tells your application to use SchoolDbContext
// and connect to a SQL Server database using the "DefaultConnection" string
builder.Services.AddDbContext<SchoolDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
// These lines set up how requests are processed (e.g., error handling, routing)
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection(); // Redirects HTTP requests to HTTPS
app.UseStaticFiles();     // Serves static files like CSS, JavaScript, images

app.UseRouting();         // Matches incoming requests to endpoints (like controller actions)

app.UseAuthorization();   // Handles user authorization (if you add authentication later)

// Maps default routing for MVC controllers (e.g., /Home/Index)
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run(); // Starts the application