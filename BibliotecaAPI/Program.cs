using BibliotecaAPI.Data;
using BibliotecaAPI.Middlewares;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// SERVICES AREA

builder.Services.AddControllers().AddJsonOptions(
    options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddDbContext<AplicationDbContext>(options => options.UseSqlServer("name=DefaultConnection"));


var app = builder.Build();

// MIDDLEWARERS AREA

app.UseLoggerRequest();
app.UseBlockedPath();

app.MapControllers();

app.Run();
