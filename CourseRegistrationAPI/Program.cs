using CourseRegistrationAPI.Data;
using CourseRegistrationAPI.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using System.Text;
using Serilog;
using CourseRegistrationAPI.Middleware;

var builder = WebApplication.CreateBuilder(args);

//configure serilog
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Hour)
    .CreateLogger();

//register serilog
builder.Host.UseSerilog();

//db injection

builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//configure identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();
   

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


// Configure JWT AUthentication

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });


//configure authorization
builder.Services.AddAuthorization();



var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>(); // for globar error handling

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapScalarApiReference();
    app.MapOpenApi();
}




// creation of roles on startup
using (var scope = app.Services.CreateScope())
{
    var rolemanager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var usermanager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

    string[] roles = {"Admin", "Student", "Instructor" };

   

    foreach (var role in roles)
    {
        if(! await rolemanager.RoleExistsAsync(role))
        {
            await rolemanager.CreateAsync(new IdentityRole(role));
        }
    }

   

}


app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();


app.Run();
