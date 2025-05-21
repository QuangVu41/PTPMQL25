using DemoMVC.Data;
using DemoMVC.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using DemoMVC.Models.Process;
using VicemMVCIdentity.Models.Process;
using Microsoft.AspNetCore.Identity.UI.Services;

var builder = WebApplication.CreateBuilder(args);
// builder.Services.AddTransient<WorkerSeeder>();

builder.Services.AddOptions();

var mailSettings = builder.Configuration.GetSection("MailSettings");
builder.Services.Configure<MailSettings>(mailSettings);
builder.Services.AddTransient<IEmailSender, SendMailService>();

builder.Services.AddRazorPages();
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
    options.SignIn.RequireConfirmedEmail = true;
    options.SignIn.RequireConfirmedPhoneNumber = false;
}).AddDefaultTokenProviders().AddEntityFrameworkStores<ApplicationDbContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Role", policy => policy.RequireClaim("Role", "AdminOnly"));
    options.AddPolicy("Permission", policy => policy.RequireClaim("Role", "EmployeeOnly"));
});

builder.Services.Configure<IdentityOptions>(options =>
{
    // Lockout settings
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
    options.Lockout.MaxFailedAccessAttempts = 1;
    options.Lockout.AllowedForNewUsers = true;
    // Password settings
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
    // Signin settings
    options.SignIn.RequireConfirmedEmail = true;
    options.SignIn.RequireConfirmedAccount = true;
    options.SignIn.RequireConfirmedPhoneNumber = false;
    // User settings
    options.User.RequireUniqueEmail = true;
});
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // For production use only
    options.Cookie.SameSite = SameSiteMode.Lax; // For production use only
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
    options.LoginPath = "/Identity/Account/Login";
    options.LogoutPath = "/Identity/Account/Logout";
    options.SlidingExpiration = true;
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string is not found!"));
});
// builder.Services.Configure<FormOptions>(options =>
// {
//     // Set overall form limit - 50MB
//     options.MultipartBodyLengthLimit = 52428800;

//     // Buffer limit
//     options.MultipartHeadersLengthLimit = 32768;

//     // Value count limit - how many form values are allowed
//     options.ValueCountLimit = 10240;
// });

var app = builder.Build();

// using (var scope = app.Services.CreateAsyncScope())
// {
//     var services = scope.ServiceProvider;
//     var seeder = services.GetRequiredService<WorkerSeeder>();
//     seeder.SeedWorkers(100);
// }

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages();

app.Run();

// dotnet aspnet-codegenerator controller -name YourController -m 
// YourModel -dc YourNamespce.ApplicationDbContext --relativeFolderPath Controllers --useDefaultLayout --referenceScriptLibraries --databaseProvider sqlite