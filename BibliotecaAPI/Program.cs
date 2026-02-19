using BibliotecaAPI.Data;
using BibliotecaAPI.Entities;
using BibliotecaAPI.Middlewares;
using BibliotecaAPI.Services;
using BibliotecaAPI.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// SERVICES AREA
var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>();

builder.Services.AddDataProtection();

builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddDbContext<AplicationDbContext>(options => options.UseSqlServer("name=DefaultConnection"));
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(optionsCors =>
    {
        optionsCors.WithOrigins(allowedOrigins!).AllowAnyMethod().AllowAnyHeader(); 
    });
});
// AUTH & AUTORIZE CONFIG
builder.Services.AddIdentityCore<User>()

    .AddEntityFrameworkStores<AplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<UserManager<User>>();
builder.Services.AddScoped<SignInManager<User>>();
builder.Services.AddTransient<IUsersService, UsersService>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthentication().AddJwtBearer(o =>
{
    o.MapInboundClaims = false;
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["keyjwt"]!)),
        ClockSkew = TimeSpan.Zero

    };
});

builder.Services.AddAuthorization(o =>
{
    o.AddPolicy("isadmin", policy => policy.RequireClaim("isadmin"));
});


// MIDDLEWARERS AREA

var app = builder.Build();

app.Use(async (context, next) =>
{
    context.Response.Headers.Append("Hi", "Value");
    await next();
});

app.UseLoggerRequest();
app.UseBlockedPath();

app.UseCors();

app.MapControllers();

app.Run();
