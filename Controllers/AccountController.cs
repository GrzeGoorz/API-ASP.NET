

using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        
        // Prywatne pole przechowujące kontekst bazy danych
        private readonly DataContext _context;

         // Konstruktor kontrolera, przyjmujący jako parametr kontekst bazy danych
        public AccountController(DataContext context)
        {
            _context = context;
            
        }

        // Akcja obsługująca żądanie rejestracji nowego użytkownika
        [HttpPost("register")] // POST: /api/account/register
        public async Task<ActionResult<AppUser>> Register(RegisterDto registerDto)
        {
            if (await UserExists(registerDto.Username)) return BadRequest("Nazwa jest zajęta");

            // Utworzenie obiektu do generowania skrótów kryptograficznych za pomocą algorytmu HMACSHA512
            using var hmac = new HMACSHA512();
            
            // Utworzenie nowego obiektu użytkownika
            var user = new AppUser
            {
                UserName = registerDto.Username.ToLower(),
                // Obliczenie skrótu hasła i przypisanie do właściwości PasswordHash
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                 // Przypisanie klucza do właściwości PasswordSalt
                PasswordSalt = hmac.Key
            };

            // Dodanie użytkownika do kontekstu bazy danych
            _context.Users.Add(user);
            // Zapisanie zmian w bazie danych
            await _context.SaveChangesAsync();
            // Zwrócenie utworzonego użytkownika jako wyniku akcji
            return user;
        }

        // Prywatna metoda sprawdzająca, czy użytkownik o podanej nazwie już istnieje
        private async Task<bool> UserExists(string username)
        {   
            // Sprawdzenie, czy istnieje użytkownik o podanej nazwie w bazie danych
            return await _context.Users.AnyAsync(x => x.UserName == username.ToLower());
        }
    }
}