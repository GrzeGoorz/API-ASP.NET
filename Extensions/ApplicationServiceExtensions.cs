
using API.Data;
using API.Interfaces;
using API.Services;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions
{
    public static class ApplicationServiceExtensions
    {
       
        /// Metoda rozszerzająca IServiceCollection, dodająca usługi aplikacji do kontenera serwisowego.
        /// param name="services">Interfejs IServiceCollection do rozszerzenia.
        /// param name="config">Konfiguracja aplikacji, zawierająca m.in. łączenie do bazy danych.
        /// <returns>Zmodyfikowany IServiceCollection z dodanymi usługami aplikacji.
        public static IServiceCollection AddApplicationServices(this IServiceCollection services,
            IConfiguration config)
        {
            // Dodaje kontekst bazy danych (DbContext) do kontenera serwisowego.
            services.AddDbContext<DataContext>(opt => 
            {   
                // Konfiguruje ustawienia DbContext, w tym łączenie z bazą danych SQLite.
                opt.UseSqlite(config.GetConnectionString("DefaultConnection"));
            });

            // Dodaje obsługę CORS (Cross-Origin Resource Sharing).
            services.AddCors();
            // Dodaje Scoped implementację interfejsu ITokenService i jego implementację TokenService do kontenera serwisowego.
            services.AddScoped<ITokenService, TokenService>();
            // Zwraca zmodyfikowany IServiceCollection z dodanymi usługami aplikacji.
            return services;
        }
    }
}