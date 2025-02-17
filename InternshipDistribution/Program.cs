
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

            builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
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

            // Включаем Swagger только в режиме разработки
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Internship Distribution API v1");
                    options.RoutePrefix = string.Empty; // Открывать Swagger по умолчанию на главной странице
                });
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();

            app.MapControllers();

            // Главная страница API
            app.MapGet("/", () => "Welcome to Internship Distribution API! Use /swagger to view API documentation.");

            app.Run();
        }
    }
}
