    // Program.cs located in your schoolmanagement project root

    using Microsoft.EntityFrameworkCore;
    using Microsoft.AspNetCore.Identity;
    using schoolmanagement.Data;
    using schoolmanagement.Models.Auth;
    using Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection; // Essential for builder.Services extension methods

    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.
    builder.Services.AddControllersWithViews();
    builder.Services.AddRazorPages();

    // Configure Entity Framework Core DbContext
    builder.Services.AddDbContext<SchoolDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.")));

    // Add ASP.NET Core Identity services
    builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
    {
        options.SignIn.RequireConfirmedAccount = false; // For local dev, can be true for production
    })
    .AddRoles<IdentityRole>() // Enabled roles: This line is crucial for role management
    .AddEntityFrameworkStores<SchoolDbContext>();

    var app = builder.Build();

    // === CRITICAL: Call the role and admin user seeding method HERE ===
    // This method will ensure roles and a default admin user exist in the database.
    // It runs when the application starts.
    await SeedRolesAndAdminAsync(app);
    // =================================================================

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseMigrationsEndPoint();
    }
    else
    {
        app.UseExceptionHandler("/Home/Error");
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseRouting();

    // IMPORTANT: Authentication middleware must come before Authorization middleware
    app.UseAuthentication();
    app.UseAuthorization();

    // Maps Razor Pages (essential for Identity area pages like /Identity/Account/Login)
    app.MapRazorPages();

    // Maps default routing for MVC controllers (e.g., /Home/Index, /Students/Index)
    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    app.Run();


    // =========================================================================
    // HELPER METHOD: Seeds default roles and an initial admin user into the database.
    // This method is defined outside the main program flow but within the file.
    // =========================================================================
    async Task SeedRolesAndAdminAsync(WebApplication application)
    {
        // Create a service scope to get isolated services (UserManager, RoleManager)
        using var scope = application.Services.CreateScope();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        // Define the roles you want to create
        string[] roles = { "Admin", "Teacher", "Student" };

        // Ensure each role exists in the database
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                Console.WriteLine($"Creating role: {role}"); // For debugging/confirmation
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        // Create a default admin user if one doesn't already exist
        var adminEmail = "admin@school.com"; // Use a strong, unique email for your admin
        var adminPassword = "Admin@123!"; // <--- CHANGE THIS TO A VERY STRONG PASSWORD FOR PRODUCTION!

        var adminUser = await userManager.FindByEmailAsync(adminEmail);
        if (adminUser == null)
        {
            Console.WriteLine($"Creating admin user: {adminEmail}"); // For debugging/confirmation
            var newAdmin = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true, // Set to true for dev convenience, remove or handle email confirmation for production
                Name = adminEmail // FIXED: Initializing the required 'Name' property
            };

            // Attempt to create the user with the given password
            var result = await userManager.CreateAsync(newAdmin, adminPassword);

            if (result.Succeeded)
            {
                // If user creation is successful, add them to the "Admin" role
                await userManager.AddToRoleAsync(newAdmin, "Admin");
                Console.WriteLine($"Admin user '{adminEmail}' created and assigned to 'Admin' role.");
            }
            else
            {
                // Log any errors if admin user creation fails
                Console.WriteLine($"Error creating admin user '{adminEmail}':");
                foreach (var error in result.Errors)
                {
                    Console.WriteLine($"- Code: {error.Code}, Description: {error.Description}");
                }
            }
        }
        else
        {
             Console.WriteLine($"Admin user '{adminEmail}' already exists. Ensuring 'Admin' role is assigned...");
             if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
             {
                 await userManager.AddToRoleAsync(adminUser, "Admin");
                 Console.WriteLine($"Admin user '{adminEmail}' assigned to 'Admin' role.");
             }
        }
    }
    