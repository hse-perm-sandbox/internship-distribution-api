using InternshipDistribution.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace InternshipDistribution
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Регистрация CORS (добавьте ЭТОТ БЛОК)
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin() // Разрешить запросы с любых доменов
                          .AllowAnyMethod() // Разрешить все HTTP-методы (GET, POST и т.д.)
                          .AllowAnyHeader(); // Разрешить все заголовки
                });
            });

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Internship Distribution API",
                    Version = "v1",
                    Description = "API для управления распределением студентов по стажировкам."
                });
            });

            var app = builder.Build();

            // Подключение CORS (добавьте ЭТУ СТРОКУ перед UseAuthorization)
            app.UseCors("AllowAll");

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Internship Distribution API v1");
                    options.RoutePrefix = string.Empty;
                });
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            app.MapGet("/", () => "Welcome to Internship Distribution API! Use /swagger to view API documentation.");
            app.Run();
        }
    }
}