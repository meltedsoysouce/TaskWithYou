using DBKernel;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddDbContext<AppDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite($"Data Source={builder.Configuration.GetConnectionString("DefaultConnection")}"));

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
