using BibliotecaAPI.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// SERVICES AREA

builder.Services.AddControllers();

builder.Services.AddDbContext<AplicationDbContext>(options => options.UseSqlServer("name=DefaultConnection"));


var app = builder.Build();

// MIDDLEWARERS AREA

app.MapControllers();

app.Run();
