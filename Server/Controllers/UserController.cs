using FavoriteMoviesFall2024.Server.Data;
using FavoriteMoviesFall2024.Server.Models;
using FavoriteMoviesFall2024.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

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
    public async Task<IActionResult> GetMovies()
    {
        var user = await _userManager.FindByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));

        if (user is null)
        {
            return NotFound();
        }

        var movies = await _context.Users.Include(u => u.FavoriteMovies)
                        .Select(u => new UserDto
                        {
                            Id = u.Id,
                            FavoriteMovies = u.FavoriteMovies
                        })
                        .FirstOrDefaultAsync(u => u.Id == user.Id);

        return Ok(user);
        
    }

    [HttpPost]
    [Route("api/add-movie")]
    public async Task<IActionResult> AddMovie(string username, [FromBody] Movie movie)
    {
        var user = await _userManager.FindByNameAsync(username);
        if (user != null)
        {
            user.FavoriteMovies.Add(movie);
            await _context.SaveChangesAsync();
            return Ok();
        } else
        {
            return NotFound();
        }
        
    }

    [HttpPost]
    [Route("api/remove-movie")]
    public async Task<IActionResult> RemoveMovie([FromBody] Movie movie)
    {
        var user = await _userManager.FindByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));

        if (user is null) { 
            return NotFound();
        }

        var movieToRemove = _context.Users.Include(u => u.FavoriteMovies).FirstOrDefault(u => u.Id == user.Id)
                            .FavoriteMovies.FirstOrDefault(m => m.imdbId == movie.imdbId);

        _context.Movies.Remove(movieToRemove);
        _context.SaveChanges();
        return Ok();
    }
}
