using Authentication.API.Data;
using Authentication.API.Model;
using BuildingBlocks.Behaviours;
using Carter;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);


var assembly = typeof(Program).Assembly;
builder.Services.AddCarter();
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(assembly);
    config.AddOpenBehavior(typeof(ValidationBehaviour<,>));
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));
});

// Add services to the container
builder.Services.AddDbContext<AuthDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Auth_Connection"));
});

// Configure JWT Options
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("ApiSettings:JwtOptions"));
builder.Services.AddSingleton(sp => sp.GetRequiredService<IOptions<JwtOptions>>().Value);

// Add Identity
builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<AuthDbContext>()
    .AddDefaultTokenProviders();

// Enable controllers and Minimal APIs
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "Bearer";
    options.DefaultChallengeScheme = "Bearer";
})
.AddJwtBearer("Bearer", options =>
{
    var jwtOptions = builder.Configuration.GetSection("ApiSettings:JwtOptions");
    options.Authority = jwtOptions["Issuer"];
    options.Audience = jwtOptions["Audience"];
});

builder.Services.AddAuthorization();

var app = builder.Build();

// Apply pending migrations and seed admin users
ApplyMigration();
app.MapCarter();
// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// **Correct middleware order**
app.UseRouting();  
app.UseAuthentication();
app.UseAuthorization();

// **Ensure routes are mapped**
app.MapControllers();  // Required for attribute-based controllers
 

app.Run();

// **Apply Migrations & Seed Admin User**
void ApplyMigration()
{
    using (var scope = app.Services.CreateScope())
    {
        var _db = scope.ServiceProvider.GetRequiredService<AuthDbContext>();

        if (_db.Database.GetPendingMigrations().Any())
        {
            _db.Database.Migrate();
        }

        // Seed Admin & SuperAdmin Users if they don't exist
        if (!_db.Users.Any(u => u.Role == "Admin" || u.Role == "SuperAdmin"))
        {
            var adminUser = new User
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "admin",
                Email = "admin@example.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
                Role = "Admin"
            };

            var superAdminUser = new User
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "superadmin",
                Email = "superadmin@example.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("SuperAdmin@123"),
                Role = "SuperAdmin"
            };

            _db.Users.AddRange(adminUser, superAdminUser);
            _db.SaveChanges();
        }
    }
}
