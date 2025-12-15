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
        /// Adds global exception handler to the service collection
        /// </summary>
        public static IServiceCollection AddGlobalExceptionHandler(this IServiceCollection services)
        {
            services.AddExceptionHandler<GlobalExceptionHandler>();
            services.AddProblemDetails();

            return services;
        }

        // Adding of swagger services 

        public static IServiceCollection AddSwaggerServices(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(
                (options =>
                {
                    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                    options.IncludeXmlComments(xmlPath);
                })
                );
            return services;
        }

        //Addding fo SQL services
        public static IServiceCollection AddSqlServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            return services;
        }

        /// <summary>
        /// Adds application services
        /// </summary>
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
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
