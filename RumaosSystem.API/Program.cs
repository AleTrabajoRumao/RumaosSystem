using Microsoft.EntityFrameworkCore;
using RumaosSystem.Infrastructure.Persistence;
using RumaosSystem.Application.Interfaces;
using RumaosSystem.Infrastructure.Repositories;
using RumaosSystem.Application.Interface;
using RumaosSystem.Infrastructure.Repository;
using Microsoft.Extensions.Options;
var builder = WebApplication.CreateBuilder(args);

// Configuración del DbContext con logging detallado
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    options.EnableSensitiveDataLogging(); 
    options.EnableDetailedErrors(); 
    options.LogTo(Console.WriteLine, LogLevel.Information); 
});

// 👉 Inyección de dependencias para el repositorio
builder.Services.AddScoped<IChequeRepository, ChequeRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IBancoRepository, BancoRepository>();





builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost3000", policy =>
    {
        policy.WithOrigins("http://localhost:3000") 
              .AllowAnyHeader() 
              .AllowAnyMethod(); 
    });

});

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddLogging();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowLocalhost3000");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
