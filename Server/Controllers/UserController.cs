using FavoriteMoviesFall2024.Server.Data;
using FavoriteMoviesFall2024.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FavoriteMoviesFall2024.Server.Controllers;

public class UserController : Controller
{
    private readonly ApplicationDbContext _context;

    public UserController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    [Route("api/User")]
    public async Task<UserDto> GetMovies()
    {
        var user = await _context.Users.Include(u => u.FavoriteMovies)
                            .Select(u => new UserDto
                            {
                                Id = u.Id,
                                FavoriteMovies = u.FavoriteMovies
                            }).FirstOrDefaultAsync();

        return user;
    }
}
