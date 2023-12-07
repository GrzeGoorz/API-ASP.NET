using API.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Dodaje kontekst bazy danych (DbContext) do kontenera serwisowego.
builder.Services.AddDbContext<DataContext>(opt => 
{   
    // Konfiguruje ustawienia DbContext, w tym łączenie z bazą danych SQLite.
    opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});


builder.Services.AddCors();

// Tworzy instancję aplikacji ASP.NET Core na podstawie konfiguracji zdefiniowanej w obiekcie builder.
var app = builder.Build();

// Konfiguracja obsługi zapytań CORS (Cross-Origin Resource Sharing). co pozwala na żądania z innych domen niż ta, z której serwuje się zasoby. 
// W tym przypadku, pozwala na żądania z dowolnych nagłówków (AllowAnyHeader()), dowolnych metod (AllowAnyMethod()),
// oraz z określonego pochodzenia (WithOrigins("http://localhost:4200")), co jest często używane w aplikacjach internetowych na front-endzie.
app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200"));

// Mapuje kontrolery do ścieżek URL.
app.MapControllers();

// Uruchamia aplikację. Ta linia kodu jest odpowiedzialna za obsługę żądań HTTP.
app.Run();
