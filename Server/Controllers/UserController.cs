using FavoriteMoviesFall2024.Server.Data;
using FavoriteMoviesFall2024.Server.Models;
using FavoriteMoviesFall2024.Server.Services;
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
    private readonly IUserService _userService;

    public UserController(  IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    [Route("api/User")]
    public async Task<DataResponse<UserDto>> GetMovies(string? userName = null)
    {
        UserDto user = new UserDto();
        if (userName is null)
        {
            user = await _userService.GetMovies(User.Identity.Name);
        } else
        {
            user = await _userService.GetMovies(userName);
        }
        

        if (user is null)
        {
            return new DataResponse<UserDto>() { Data = new UserDto(), Succeeded = false, Message = "User Not Found." };
        }

        return new DataResponse<UserDto>(user);
        
    }

    [HttpGet]
    [Route("api/users")]
    public async Task<DataResponse<List<UserEditDto>>> GetUsers()
    {
        try
        {
            var users = await _userService.GetUsers();
            if (users is null || users.Count() == 0)
            {
                return new DataResponse<List<UserEditDto>>()
                {
                    Succeeded = false,
                    Message = "Error retrieving users"
                };
            }

            return new DataResponse<List<UserEditDto>>(users);
        }
        catch (Exception e)
        {
            Debug.WriteLine(e.Message);
            return new DataResponse<List<UserEditDto>>()
            {
                Succeeded = false,
                Message = e.Message
            };
        }
    }

    //[HttpPost]
    //[Route("api/add-movie")]
    //public async Task<IActionResult> AddMovie(string username, [FromBody] Movie movie)
    //{
    //    var user = await _userManager.FindByNameAsync(username);
    //    if (user != null)
    //    {
    //        user.FavoriteMovies.Add(movie);
    //        await _context.SaveChangesAsync();
    //        return Ok();
    //    } else
    //    {
    //        return NotFound();
    //    }

    //}

    //[HttpPost]
    //[Route("api/remove-movie")]
    //public async Task<Response> RemoveMovie([FromBody] Movie movie)
    //{
    //    try
    //    {
    //        var user = await _userManager.FindByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));

    //        if (user is null)
    //        {
    //            var roles = await _userManager.GetRolesAsync(user);
    //            if (!roles.Contains("Admin"))
    //            {
    //                return new Response("Not Allowed.");
    //            }

    //        }

    //        var movieToRemove = _context.Users.Include(u => u.FavoriteMovies).FirstOrDefault(u => u.Id == user.Id)
    //                            .FavoriteMovies.FirstOrDefault(m => m.imdbId == movie.imdbId);

    //        if (movieToRemove is null)
    //        {
    //            return new Response("Movie Not Found.");
    //        }
    //        else
    //        {
    //            _context.Movies.Remove(movieToRemove);
    //            _context.SaveChanges();
    //            return new Response(true, "Movie Removed Successfully.");
    //        }
    //    }
    //    catch (Exception e)
    //    {
    //        Debug.WriteLine(e.Message);
    //        return new Response(e.Message);
    //    }




    //}

    ////[HttpPost]
    ////[Route("api/create-admin-user")]
    ////public async Task<IActionResult> CreateAdminUser()
    ////{
    ////    try
    ////    {
    ////        string adminRole = "Admin";
    ////        string newAdminUser = "someone@example.net";
    ////        string adminUserPassword = "Admin123";

    ////        IdentityResult resRoleCreation = await _roleManager.CreateAsync(new IdentityRole { Name = adminRole, NormalizedName = adminRole.ToUpper() });
    ////        IdentityResult resUserCreate = await _userManager.CreateAsync(new ApplicationUser { UserName = newAdminUser }, adminUserPassword);
    ////        if (resRoleCreation.Succeeded && resUserCreate.Succeeded)
    ////        {
    ////            var user = await _userManager.FindByNameAsync(newAdminUser);
    ////            if (user != null)
    ////            {
    ////                await _userManager.AddToRoleAsync(user, adminRole);
    ////            }
    ////        }
    ////        return Ok();
    ////    }
    ////    catch (Exception e)
    ////    {
    ////        Debug.WriteLine(e.Message);
    ////        return BadRequest();
    ////    }
    ////}

    //[HttpGet]
    //[Route("api/toggle-enabled-user/{userId}")]
    //public async Task<Response> ToggleEnabledUser(string userId)
    //{
    //    try
    //    {
    //        var user = await _userManager.FindByIdAsync(userId);
    //        if (user is null)
    //        {
    //            return new Response("User not found.");
    //        }

    //        user.LockoutEnabled = !user.LockoutEnabled;
    //        await _userManager.UpdateAsync(user);
    //        return new Response(true, "User Enabled/Disabled.");
    //    }
    //    catch (Exception e)
    //    {
    //        Debug.WriteLine(e.Message);
    //        return new Response(e.Message);
    //    }
    //}

    //[HttpGet]
    //[Route("api/toggle-admin-role/{userId}")]
    //public async Task<Response> ToggleAdminRole(string userId)
    //{
    //    try
    //    {
    //        var user = await _userManager.FindByIdAsync(userId);
    //        if (user is null)
    //        {
    //            return new Response("User not found.");
    //        }

    //        var roles = await _userManager.GetRolesAsync(user);
    //        if (roles.Contains("Admin"))
    //        {
    //            await _userManager.RemoveFromRoleAsync(user, "Admin");
    //            return new Response(true, "User Removed from Admin.");
    //        }
    //        else
    //        {
    //            await _userManager.AddToRoleAsync(user, "Admin");
    //            return new Response(true, "User Added to Admin.");
    //        }

    //    }
    //    catch (Exception e)
    //    {
    //        Debug.WriteLine(e.Message);
    //        return new Response(e.Message);
    //    }
    //}
}
