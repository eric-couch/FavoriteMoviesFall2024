using FavoriteMoviesFall2024.Shared;
using FavoriteMoviesFall2024.Server.Models;
using FavoriteMoviesFall2024.Server.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using FavoriteMoviesFall2024.Shared.Wrapper;
using System.Diagnostics;

namespace FavoriteMoviesFall2024.Server.Services;

public class UserService : IUserService
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public UserService( ApplicationDbContext context,
                        RoleManager<IdentityRole> roleManager,
                        UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _roleManager = roleManager;
        _userManager = userManager;
    }

    public async Task<UserDto> GetMovies(string userName)
    {
        var user = await _userManager.FindByNameAsync(userName);

        if (user is null)
        {
            return new UserDto();
        }

        var movies = await _context.Users.Include(u => u.FavoriteMovies)
                        .Select(u => new UserDto
                        {
                            Id = u.Id,
                            UserName = u.UserName,
                            FavoriteMovies = u.FavoriteMovies
                        })
                        .FirstOrDefaultAsync(u => u.Id == user.Id);

        return movies;
    }

    public async Task<List<UserEditDto>> GetUsers()
    {
        try
        {
            var users = await (from u in _context.Users
                               let query = (from ur in _context.Set<IdentityUserRole<string>>()
                                            where ur.UserId.Equals(u.Id)
                                            join r in _context.Roles on ur.RoleId equals r.Id
                                            select r.Name).ToList()
                               select new UserEditDto
                               {
                                   Id = u.Id,
                                   UserName = u.UserName,
                                   Email = u.Email,
                                   EmailConfirmed = u.EmailConfirmed,
                                   LockoutEnabled = u.LockoutEnabled,
                                   Admin = query.Contains("Admin")
                               }).ToListAsync();

            return new List<UserEditDto>(users);
        }
        catch (Exception e)
        {
            Debug.WriteLine(e.Message);
            return new List<UserEditDto>();
        }
    }
}
