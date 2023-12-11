using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[Authorize]
public class UsersController : BaseApiController
{
    private readonly DataContext _context;

    public UsersController(DataContext context)
    {
        _context = context; 
    }

    [AllowAnonymous]
    // Akcja do pobierania wszystkich użytkowników
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers() 
    {
        // Pobranie wszystkich użytkowników z bazy danych
        var users = await _context.Users.ToListAsync();
        // Zwrócenie listy użytkowników jako wyniku akcji
        return users;
    }

    
     // Akcja do pobierania użytkownika o określonym id
    [HttpGet("{id}")] // /api/users/2
    public async Task<ActionResult<AppUser>> GetUser(int id)
    {
    // Pobranie użytkownika z bazy danych na podstawie id
    var user = await _context.Users.FindAsync(id);

    // Sprawdzenie, czy użytkownik o podanym identyfikatorze istnieje
    if (user == null)
    {
        return NotFound(); // Zwróć kod 404, gdy użytkownik nie istnieje
    }
    // Zwrócenie użytkownika jako wyniku udanej akcji
    return user;
    }
}
