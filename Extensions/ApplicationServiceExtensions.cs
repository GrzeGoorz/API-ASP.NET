
using API.Data;
using API.Interfaces;
using API.Services;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services,
            IConfiguration config)
        {
            // Dodaje kontekst bazy danych (DbContext) do kontenera serwisowego.
            services.AddDbContext<DataContext>(opt => 
            {   
                // Konfiguruje ustawienia DbContext, w tym łączenie z bazą danych SQLite.
                opt.UseSqlite(config.GetConnectionString("DefaultConnection"));
            });


            services.AddCors();
            services.AddScoped<ITokenService, TokenService>();

            return services;
        }
    }
}