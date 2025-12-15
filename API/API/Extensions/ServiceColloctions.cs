using API.Application.Abstractions;
using API.Application.Services;
using API.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace API.Extensions
{
    /// <summary>
    /// Extension methods for configuring services and middleware
    /// </summary>
    public static class ServiceCollections
    {
        /// <summary>
        /// Adds all application services and configurations
        /// </summary>
        public static IServiceCollection AddAllServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Controllers
            services.AddControllers();

            // Exception handling
            services.AddGlobalExceptionHandler();

            // Database
            services.AddSqlServices(configuration);

            // Application services
            services.AddApplicationServices();

            // Swagger
            services.AddSwaggerServices();

            return services;
        }

        /// <summary>
        /// Adds global exception handler to the service collection
        /// </summary>
        private static IServiceCollection AddGlobalExceptionHandler(this IServiceCollection services)
        {
            services.AddExceptionHandler<GlobalExceptionHandler>();
            services.AddProblemDetails();

            return services;
        }

        // Adding of swagger services
        private static IServiceCollection AddSwaggerServices(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

                // Only include XML comments if file exists
                if (File.Exists(xmlPath))
                {
                    options.IncludeXmlComments(xmlPath);
                }
            });
            return services;
        }

        //Addding fo SQL services
        private static IServiceCollection AddSqlServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("sqlConnection"),
                    b => b.MigrationsAssembly("API")
                          .MigrationsHistoryTable("__EFMigrationsHistory", "dbo")));

            return services;
        }

        /// <summary>
        /// Adds application services
        /// </summary>
        private static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // CRUD Services
            services.AddScoped<IIndustryService, IndustryService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IProductSubTypeService, ProductSubTypeService>();
            services.AddScoped<IConfigurationService, ConfigurationService>();

            // Excel and SQL Query Services
            services.AddScoped<IExcelService, ExcelService>();
            services.AddScoped<ISqlQueryService, SqlQueryService>();

            return services;
        }

        /// <summary>
        /// Uses global exception handler in the application pipeline
        /// </summary>
        public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler();

            return app;
        }
    }
}
