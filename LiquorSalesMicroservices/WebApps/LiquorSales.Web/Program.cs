using LiquorSales.Web.Implementations;
using Microsoft.AspNetCore.Authentication.Cookies;
using Polly;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddRefitClient<ICatalogueServices>()
    .ConfigureHttpClient(c =>
    {
        c.BaseAddress = new Uri(builder.Configuration["ApiSettings:GatewayAddress"]!);
    });

builder.Services.AddRefitClient<ICartServices>()
    .ConfigureHttpClient(c =>
    {
        c.BaseAddress = new Uri(builder.Configuration["ApiSettings:GatewayAddress"]!);
    });
builder.Services.AddRefitClient<IOrderingService>()
    .ConfigureHttpClient(c =>
    {
        c.BaseAddress = new Uri(builder.Configuration["ApiSettings:GatewayAddress"]!);
    });

builder.Services.AddRefitClient<IAuthService>()
    .ConfigureHttpClient(client =>
    {
        client.BaseAddress = new Uri("http://localhost:6005"); // Direct Backend URL for AuthApi
    });

builder.Services.AddScoped<ILoadCartServices, LoadCartService>();

builder.Services.AddRazorPages()
    .AddRazorRuntimeCompilation()
    .AddDataAnnotationsLocalization()
    .AddViewLocalization();

builder.Services.AddControllersWithViews()
    .AddDataAnnotationsLocalization()
    .AddViewLocalization();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});

builder.Services.AddHttpClient("MyClient")
    .AddTransientHttpErrorPolicy(policy =>
        policy.WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(retryAttempt)))
    .AddTransientHttpErrorPolicy(policy =>
        policy.CircuitBreakerAsync(5, TimeSpan.FromMinutes(1)));

builder.Services.AddHttpContextAccessor();

builder.Services.AddAuthentication("Cookies")
    .AddCookie("Cookies", options =>
    {
        options.LoginPath = "/Login";  
        options.LogoutPath = "/Logout";
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    });

builder.Services.AddAuthorization();

builder.Services.AddHttpContextAccessor(); // Needed for checking authentication in views

var app = builder.Build();
  
app.UseSession();

// Configure the HTTP request pipeline.

app.UseExceptionHandler("/Error");  

if (app.Environment.IsDevelopment())
{
   app.UseDeveloperExceptionPage();
    app.UseStatusCodePagesWithReExecute("/Error", "?statusCode={0}");  

    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.Use(async (context, next) =>
{
    try
    {
        await next();
    }
    catch (UnauthorizedAccessException)
    {
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        await context.Response.WriteAsync("Unauthorized access. Please login.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Exception: {ex.Message}\nStackTrace: {ex.StackTrace}"); // Log the exception details
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        await context.Response.WriteAsync($"Error: {ex.Message}"); // Return the actual error message
    }
});




app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
