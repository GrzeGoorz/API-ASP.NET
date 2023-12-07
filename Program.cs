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

// Buduje obiekt aplikacji
var app = builder.Build();

// Włącza przekierowanie na protokół HTTPS
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
