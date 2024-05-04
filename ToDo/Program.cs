using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;
using ToDo.Data;
using ToDo.Services;

var builder = WebApplication.CreateBuilder(args);

// Configure services.
builder.Services.AddDbContext<ToDoContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ToDoContext") ?? throw new InvalidOperationException("Connection string 'ToDoContext' not found.")));

builder.Services.AddControllersWithViews();
builder.Services.AddScoped<INoteService, NoteService>();
builder.Services.AddSingleton<CommonLocalizationService>();

// Add NToastNotify Toastr services.
builder.Services.AddMvc().AddNToastNotifyToastr();

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[]
    {
        new CultureInfo("en-GB"),
        new CultureInfo("pt-PT")
    };

    options.DefaultRequestCulture = new RequestCulture("en-GB");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});

// Build the app.
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();  // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
}

app.UseNToastNotify();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseRequestLocalization();

app.Use(async (context, next) =>
{
    var queryCulture = context.Request.Query["culture"];
    if (!string.IsNullOrWhiteSpace(queryCulture))
    {
        var culture = new CultureInfo(queryCulture);
        context.Response.Cookies.Append(
            CookieRequestCultureProvider.DefaultCookieName,
            CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
            new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
        );
        // Log the current culture
        Console.WriteLine("Current Culture: " + CultureInfo.CurrentCulture.Name);
    }
    Console.WriteLine($"Current Culture: {CultureInfo.CurrentCulture.Name}");
    Console.WriteLine($"Current UICulture: {CultureInfo.CurrentUICulture.Name}");
    await next();
});

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});

app.Run();
