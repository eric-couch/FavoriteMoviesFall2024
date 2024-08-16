using FavoriteMoviesFall2024.Server.Data;
using FavoriteMoviesFall2024.Server.Models;
using FavoriteMoviesFall2024.Shared;
using FavoriteMoviesFall2024.Shared.Wrapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;

namespace FavoriteMoviesFall2024.Server.Controllers;

[ApiController]
public class UserController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public UserController(  ApplicationDbContext context, 
                            RoleManager<IdentityRole> roleManager,
                            UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _roleManager = roleManager;
        _userManager = userManager;
    }

    [HttpGet]
    [Route("api/User")]
    public async Task<DataResponse<UserDto>> GetMovies()
    {
        //var user = await _userManager.FindByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var user = await _userManager.FindByNameAsync(User.Identity.Name);

        if (user is null)
        {
            return new DataResponse<UserDto>() { Data = new UserDto(), Succeeded = false, Message = "User Not Found." };
        }

        var movies = await _context.Users.Include(u => u.FavoriteMovies)
                        .Select(u => new UserDto
                        {
                            Id = u.Id,
                            FavoriteMovies = u.FavoriteMovies
                        })
                        .FirstOrDefaultAsync(u => u.Id == user.Id);

        return new DataResponse<UserDto>(movies);
        
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
    public async Task<Response> RemoveMovie([FromBody] Movie movie)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (user is null)
            {
                var roles = await _userManager.GetRolesAsync(user);
                if (!roles.Contains("Admin"))
                {
                    return new Response("Not Allowed.");
                }
                
            }

            var movieToRemove = _context.Users.Include(u => u.FavoriteMovies).FirstOrDefault(u => u.Id == user.Id)
                                .FavoriteMovies.FirstOrDefault(m => m.imdbId == movie.imdbId);

            if (movieToRemove is null)
            {
                return new Response("Movie Not Found.");
            }
            else
            {
                _context.Movies.Remove(movieToRemove);
                _context.SaveChanges();
                return new Response(true, "Movie Removed Successfully.");
            }
        }
        catch (Exception e)
        {
            Debug.WriteLine(e.Message);
            return new Response(e.Message);
        }




    }

    [HttpPost]
    [Route("api/create-admin-user")]
    public async Task<IActionResult> CreateAdminUser()
    {
        try
        {
            string adminRole = "Admin";
            string newAdminUser = "someone@example.net";
            string adminUserPassword = "Admin123";

            IdentityResult resRoleCreation = await _roleManager.CreateAsync(new IdentityRole { Name = adminRole, NormalizedName = adminRole.ToUpper() });
            IdentityResult resUserCreate = await _userManager.CreateAsync(new ApplicationUser { UserName = newAdminUser }, adminUserPassword);
            if (resRoleCreation.Succeeded && resUserCreate.Succeeded)
            {
                var user = await _userManager.FindByNameAsync(newAdminUser);
                if (user != null)
                {
                    await _userManager.AddToRoleAsync(user, adminRole);
                }
            }
            return Ok();
        }
        catch (Exception e)
        {
            Debug.WriteLine(e.Message);
            return BadRequest();
        }
    }
}
