using Microsoft.EntityFrameworkCore;
using Ecommerce_App.Areas.Identity.Data;
using Domain.Interface;
using Microsoft.Extensions.Localization;
using Ecommerce_App;
using Microsoft.AspNetCore.Mvc.Razor;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Serilog;
using YourApplication.Infrastructure.Logging;
using Domain.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.CookiePolicy;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("Ecommerce_AppContextConnection") ?? throw new InvalidOperationException("Connection string 'Ecommerce_AppContextConnection' not found.");

builder.Host.UseSerilog();

builder.Services.AddDbContext<Ecommerce_App.Areas.Identity.Data.Ecommerce_AppContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<Ecommerce_AppUser>(options => options.SignIn.RequireConfirmedAccount = false)
	.AddRoles<IdentityRole>()
	.AddEntityFrameworkStores<Ecommerce_App.Areas.Identity.Data.Ecommerce_AppContext>();

builder.Services.AddDbContext<Ecommerce_App.Areas.Identity.Data.Ecommerce_AppContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Ecommerce_AppContextConnection"))
);

builder.Services.AddDbContext<Infrastructure.Data.Ecommerce_AppContext>(options =>
{
	options.UseSqlServer(builder.Configuration.GetConnectionString("Ecommerce_AppContextConnection"));
});


// Add services to the container.

var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
var logFilePath = Path.Combine(baseDirectory, "Logs", "log.txt");
Directory.CreateDirectory(Path.Combine(baseDirectory, "Logs"));

var serviceProvider = builder.Services.BuildServiceProvider();
var logger = serviceProvider.GetRequiredService<ILogger<LoggerService>>();
var dbContext = serviceProvider.GetRequiredService<Infrastructure.Data.Ecommerce_AppContext>();

builder.Services.AddSingleton<ILoggerService>(provider =>
    new LoggerService(logFilePath, logger, dbContext, provider.GetRequiredService<IServiceScopeFactory>()));


builder.Services.AddScoped<ILookUpPropertyService, LookUpPropertyService>();
builder.Services.AddScoped<ILookUpTypeService, LookUpTypeService>();
builder.Services.AddScoped<IRoomTypeService,RoomTypeService>();
builder.Services.AddScoped<IRoomService, RoomService>();
builder.Services.AddScoped<IRoomImageService, RoomImageService>();
builder.Services.AddScoped<ILookUpTypeService, LookUpTypeService>();
builder.Services.AddScoped<IReservationService, ReservationService>();
builder.Services.AddScoped<IEscortService, EscortService>();
builder.Services.AddScoped<IAnalyticService, AnalyticService>();
builder.Services.AddScoped<IVisitorService, VisitorService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddControllersWithViews();
builder.Services.AddLocalization();

builder.Services.AddSingleton<IStringLocalizerFactory, JsonStringLocalizerFactory>();
builder.Services.AddMvcCore()
	.AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
	.AddDataAnnotationsLocalization(option =>
	{
		option.DataAnnotationLocalizerProvider = (type, factory) =>
		factory.Create(typeof(JsonStringLocalizerFactory));
	});

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
	var supportedCultures = new[]
	{
		new CultureInfo("en-US"),
		new CultureInfo("ar-JO")
	};

	options.DefaultRequestCulture = new RequestCulture(culture: supportedCultures[0], uiCulture: supportedCultures[0]);
	options.SupportedCultures = supportedCultures;
	options.SupportedUICultures = supportedCultures;
});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.IsEssential = true;
    options.Cookie.SameSite = SameSiteMode.Strict;
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromDays(30);
    options.SlidingExpiration = true;
    options.Cookie.IsEssential = true;
});

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.MinimumSameSitePolicy = SameSiteMode.None;
    options.HttpOnly = HttpOnlyPolicy.Always;
    options.Secure = CookieSecurePolicy.Always;
});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.SameSite = SameSiteMode.None;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});

builder.Services.AddAntiforgery(options =>
{
    options.Cookie.SameSite = SameSiteMode.None;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
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

var supportedCultures = new[] { "en-US", "ar-JO" };
var localizationOptions = new RequestLocalizationOptions()
	.SetDefaultCulture(supportedCultures[0])
	.AddSupportedCultures(supportedCultures)
	.AddSupportedUICultures(supportedCultures);

app.UseRequestLocalization(localizationOptions);

app.UseAuthorization();


app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute("NotFound404", "{*url}", new { controller = "Base", action = "NotFound404" });


using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<Ecommerce_AppUser>>();

    app.UseMiddleware<VisitorCountMiddleware>();

    var roles = new[] { "Admin", "User", "Employee", "Guest" };
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }

    var adminEmail = "test@gmail.com";
    var guestEmail = "guest@gmail.com";
    var adminUser = await userManager.FindByEmailAsync(adminEmail);
    var guestUser = await userManager.FindByEmailAsync(guestEmail);
    if (adminUser == null)
    {
        var admin = new Ecommerce_AppUser
        {
            FirstName = "Admin",
            UserName = adminEmail,
            Email = adminEmail,
        };
        await userManager.CreateAsync(admin, "Admin123@");
        await userManager.AddToRoleAsync(admin, "Admin");
    }
    if (guestUser == null)
    {
        var guest = new Ecommerce_AppUser
        {
            FirstName = "Guest",
            UserName = guestEmail,
            Email = guestEmail,
        };
        await userManager.CreateAsync(guest, "Guest123@");
        await userManager.AddToRoleAsync(guest, "Guest");
    }
}

app.MapRazorPages();

app.Run();
