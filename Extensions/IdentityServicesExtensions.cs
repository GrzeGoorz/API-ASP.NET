using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace API.Extensions
{
    public static class IdentityServicesExtensions
    {
        /// Metoda rozszerzająca IServiceCollection, dodająca i konfigurująca usługi związane z uwierzytelnianiem opartym na tokenie JWT.
        /// param name="services" Interfejs IServiceCollection do rozszerzenia
        /// param name="config" Konfiguracja aplikacji, zawierająca klucz tokena
        /// <returns>Zmodyfikowany IServiceCollection z dodanymi usługami uwierzytelniania.
        public static IServiceCollection AddIdentityServices(this IServiceCollection services,
            IConfiguration config)
            {
                // Dodanie schematu uwierzytelniania JWT
                services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    // Konfiguracja parametrów walidacji tokena JWT
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        // Wymaga weryfikacji klucza podpisywania
                        ValidateIssuerSigningKey = true,
                        // Klucz podpisywania pobierany z konfiguracji
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding
                        .UTF8.GetBytes(config["TokenKey"])),
                        // Wyłącza weryfikację nadawcy (Issuer) ( nie będzie sprawdzać, czy token JWT został wygenerowany dla konkretnego nadawcy (wydawcy)
                        ValidateIssuer = false,
                        // Wyłącza weryfikację odbiorcy (Audience) ( nie będzie sprawdzać, czy token JWT został wygenerowany dla konkretnego odbiorcy.)
                        ValidateAudience = false
                        //Jednakże, w środowiskach produkcyjnych, szczególnie w systemach wymagających bezpieczeństwa, zazwyczaj zaleca się weryfikację nadawcy i odbiorcy,
                    };
                });
                // Zwraca zmodyfikowany IServiceCollection z dodanymi usługami uwierzytelniania
                return services;
            }
    }
}