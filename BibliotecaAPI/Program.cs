using BibliotecaAPI.Data;
using BibliotecaAPI.Middlewares;
using BibliotecaAPI.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// SERVICES AREA
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddDbContext<AplicationDbContext>(options => options.UseSqlServer("name=DefaultConnection"));

// AUTH & AUTORIZE CONFIG
builder.Services.AddIdentityCore<IdentityUser>()
    .AddEntityFrameworkStores<AplicationDbContext>()
    .AddDefaultTokenProviders();
builder.Services.AddScoped<UserManager<IdentityUser>>();
builder.Services.AddScoped<SignInManager<IdentityUser>>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthentication().AddJwtBearer(o =>
{
    o.MapInboundClaims = false; // not change autoclaim
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


var app = builder.Build();

// MIDDLEWARERS AREA

app.UseLoggerRequest();
app.UseBlockedPath();

app.MapControllers();

app.Run();
