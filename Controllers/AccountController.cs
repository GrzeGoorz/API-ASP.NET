

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

        //REJESTRACJA UŻYTKOWNIKA
        [HttpPost("register")] // POST: /api/account/register
        public async Task<ActionResult<AppUser>> Register(RegisterDto registerDto)
        {
            // Sprawdzenie, czy użytkownik o podanej nazwie już istnieje
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

        //LOGOWANIE UŻYTKOWNIKA
        [HttpPost("login")]
        public async Task<ActionResult<AppUser>> Login(LoginDto loginDto)
        {
            // Pobranie użytkownika z bazy danych na podstawie nazwy użytkownika
            var user = await _context.Users.SingleOrDefaultAsync(x => 
                x.UserName == loginDto.Username);
            
            // Sprawdzenie, czy użytkownik o podanej nazwie istnieje
            if (user == null) return Unauthorized("Zła nazwa użytkownika");

             // Utworzenie obiektu HMACSHA512, używając soli (PasswordSalt) z bazy danych
            using var hmac = new HMACSHA512(user.PasswordSalt);

            // Obliczenie skrótu hasła podanego przez użytkownika
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            // Porównanie obliczonego skrótu hasła z zapisanym skrótem w bazie danych
            for (int i = 0; i < computedHash.Length; i++)
            { //computedHash hasło z okna logowania użytkownika jest hashowane, solone i porównywane z haslem z bazy danych user.PasswordHash
                if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("Złe hasło");
            }
            // Zwrócenie użytkownika jako wyniku udanej autentykacji
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