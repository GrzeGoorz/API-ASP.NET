using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Entities;
using API.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace API.Services
{
    public class TokenService : ITokenService
    {
        private readonly SymmetricSecurityKey _key;


         // Konstruktor klasy TokenService, który przyjmuje konfigurację i inicjalizuje klucz do podpisu tokenów.
         // Klucz ten jest używany do zabezpieczenia tokenów, zapewniając, że są one autentyczne i niezmienione.
        public TokenService(IConfiguration config)
        {
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
        }


        // Metoda CreateToken generuje JWT (JSON Web Token) na podstawie informacji o użytkowniku.
        // Tworzenie tokena zawierającego informacje o użytkowniku, takie jak jego nazwa użytkownika. 
        //Token ten jest podpisywany kluczem, co umożliwia weryfikację autentyczności tokena w przyszłości.
        public string CreateToken(AppUser user)
        {
            // Tworzenie listy roszczeń (claims), zawierającej informacje o użytkowniku (w tym nazwę użytkownika).
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.NameId, user.UserName)
            };

            // Tworzenie danych do podpisu tokena (credentials) przy użyciu klucza i algorytmu HMACSHA512.
            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            // Konfiguracja opisująca token, zawierająca m.in. podmiot (claims), datę wygaśnięcia i dane do podpisu.
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = creds

            };

            // Tworzenie obiektu obsługującego JWT.
            var tokenHandler = new JwtSecurityTokenHandler();

             // Tworzenie tokena na podstawie opisu.
            var token = tokenHandler.CreateToken(tokenDescriptor);

            // Zwracanie JWT jako ciągu tekstowego.
            return tokenHandler.WriteToken(token);
        }   
    }
}