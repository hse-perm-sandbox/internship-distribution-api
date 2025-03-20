using InternshipDistribution.Filters;
using InternshipDistribution.Models;
using InternshipDistribution.Repositories;
using InternshipDistribution.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace InternshipDistribution
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.Configure<FormOptions>(options =>
            {
                options.MultipartBodyLengthLimit = 104857600; // 100 MB (пытался решить ошибку с загрузкой файлом в дебаг версии)
            });

            builder.WebHost.ConfigureKestrel(options =>
            {
                options.Limits.MaxRequestBodySize = 104857600; // 100 MB
            });

            DotNetEnv.Env.Load(Path.Combine("..", ".env"));

            var secretKey = Environment.GetEnvironmentVariable("JWT_SECRET_KEY");
            if (!string.IsNullOrEmpty(secretKey))
                builder.Configuration["Jwt:SecretKey"] = secretKey;

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin() // Разрешить запросы с любых доменов
                          .AllowAnyMethod() // Разрешить все HTTP-методы (GET, POST и т.д.)
                          .AllowAnyHeader() // Разрешить все заголовки
                          .WithExposedHeaders("Content-Disposition");
                });
            });

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = "InternshipDistributionBackend", // Совпадает с токеном
                    ValidateAudience = true,
                    ValidAudience = "InternshipDistributionFrontend", // Совпадает с токеном
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]))
                };

                options.Events = new JwtBearerEvents
                {
                    OnForbidden = async context =>
                    {
                        context.Response.StatusCode = 403;
                        context.Response.ContentType = "application/json";

                        var endpoint = context.HttpContext.GetEndpoint();
                        var requireRole = endpoint?.Metadata
                            .GetMetadata<AuthorizeAttribute>()?.Roles;

                        var policy = endpoint?.Metadata.GetMetadata<AuthorizeAttribute>()?.Policy;
                        var errorMessage = policy switch
                        {
                            "RequireStudent" => "Требуется роль Student",
                            "RequireManager" => "Требуется роль Manager",
                            _ => "Доступ запрещен"
                        };

                        await context.Response.WriteAsync($"{{\"error\": \"{errorMessage}\"}}");
                    }
                };
            });

            builder.Services.AddAuthorization(options =>
            {
                // Требует, чтобы пользователь был авторизован (любая роль)
                options.FallbackPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();

                options.AddPolicy("RequireManager", policy =>
                    policy.RequireRole("Manager"));

                options.AddPolicy("RequireStudent", policy =>
                    policy.RequireRole("Student"));
            });

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<UserRepository>();
            builder.Services.AddScoped<AuthService>();
            builder.Services.AddScoped<JwtService>();
            builder.Services.AddScoped<BCryptPasswordHasher>();
            builder.Services.AddScoped<UserService>();
            builder.Services.AddScoped<StudentService>();
            builder.Services.AddScoped<StudentRepository>();
            builder.Services.AddScoped<FileStorageService>();
            builder.Services.AddScoped<PasswordGeneratorService>();


            builder.Services.AddControllers().AddNewtonsoftJson();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Internship Distribution API",
                    Version = "v1",
                    Description = "API для управления распределением студентов по стажировкам."
                });

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });

                options.SchemaFilter<EnumSchemaFilter>();
            });

            var app = builder.Build();


            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<ApplicationDbContext>();
                    var hasher = services.GetRequiredService<BCryptPasswordHasher>();

                    DbInitializer.Initialize(context, hasher);
                }
                catch (Exception ex)
                {
                    throw new Exception("Ошибка инициализации БД: " + ex.Message);
                }
            }

            // Подключение CORS 
            app.UseCors("AllowAll");

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Intersnship Distribution API v1");
                    options.RoutePrefix = "api/docs/swagger";
                });
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            app.MapGet("/", context =>
            {
                context.Response.Redirect("/api/docs/swagger");
                return Task.CompletedTask;
            }); 
            app.Run();
        }
    }
}