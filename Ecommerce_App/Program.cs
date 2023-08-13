using Microsoft.EntityFrameworkCore;
using Ecommerce_App.Areas.Identity.Data;
using Domain.DTO_s;
using Domain.Interface;
using Ecommerce_App.Areas.Identity.Pages.Account;
using Microsoft.Extensions.Localization;
using Ecommerce_App;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Build.Framework;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;
using Microsoft.AspNetCore.Hosting;
using YourApplication.Infrastructure.Logging;
using Microsoft.Extensions.Logging.AzureAppServices;
using Microsoft.Extensions.DependencyInjection;
using Domain.Service;
using Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("Ecommerce_AppContextConnection") ?? throw new InvalidOperationException("Connection string 'Ecommerce_AppContextConnection' not found.");

builder.Host.UseSerilog();

builder.Services.AddDbContext<Ecommerce_App.Areas.Identity.Data.Ecommerce_AppContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<Ecommerce_AppUser>(options => options.SignIn.RequireConfirmedAccount = false)
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


builder.Services.AddScoped<IRoomTypeService, RoomTypeService>();
builder.Services.AddScoped<IRoomService, RoomService>();
builder.Services.AddScoped<IRoomImageService, RoomImageService>();
builder.Services.AddScoped<ISettingService, SettingService>();
builder.Services.AddScoped<IReservationService, ReservationService>();
builder.Services.AddScoped<IEscortService, EscortService>();
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

app.MapRazorPages();

app.Run();
