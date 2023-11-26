using Microsoft.EntityFrameworkCore;
using OpenMovies.Data;
using OpenMovies.Repositories;
using OpenMovies.Services;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlite(configuration.GetConnectionString("Default"));
});

builder.Services.AddScoped<MovieRepository>();
builder.Services.AddScoped<DirectorRepository>();
builder.Services.AddScoped<CategoryRepository>();

builder.Services.AddScoped<MovieService>();
builder.Services.AddScoped<DirectorService>();
builder.Services.AddScoped<CategoryService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
