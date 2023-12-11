using System.Text;
using API.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;

using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);


// Tworzy instancję aplikacji ASP.NET Core na podstawie konfiguracji zdefiniowanej w obiekcie builder.
var app = builder.Build();

// Konfiguracja obsługi zapytań CORS (Cross-Origin Resource Sharing). co pozwala na żądania z innych domen niż ta, z której serwuje się zasoby. 
// W tym przypadku, pozwala na żądania z dowolnych nagłówków (AllowAnyHeader()), dowolnych metod (AllowAnyMethod()),
// oraz z określonego pochodzenia (WithOrigins("http://localhost:4200")), co jest często używane w aplikacjach internetowych na front-endzie.
app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200"));

app.UseAuthentication();
app.UseAuthorization();
// Mapuje kontrolery do ścieżek URL.
app.MapControllers();

// Uruchamia aplikację. Ta linia kodu jest odpowiedzialna za obsługę żądań HTTP.
app.Run();
