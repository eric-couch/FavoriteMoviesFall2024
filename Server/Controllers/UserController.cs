using FavoriteMoviesFall2024.Server.Data;
using FavoriteMoviesFall2024.Server.Models;
using FavoriteMoviesFall2024.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FavoriteMoviesFall2024.Server.Controllers;

public class UserController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public UserController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [HttpGet]
    [Route("api/User")]
    public async Task<IActionResult> GetMovies(string userId)
    {
        if (userId is not null)
        {
            var user = await _userManager.FindByNameAsync(userId);

            var movies = await _context.Users.Include(u => u.FavoriteMovies)
                            .Select(u => new UserDto
                            {
                                Id = u.Id,
                                FavoriteMovies = u.FavoriteMovies
                            })
                            .FirstOrDefaultAsync(u => u.Id == user.Id);

            return Ok(user);
        } else
        {
            return NotFound();
        }
        
    }
}
