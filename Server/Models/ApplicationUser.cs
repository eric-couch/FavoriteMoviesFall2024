using FavoriteMoviesFall2024.Shared;
using Microsoft.AspNetCore.Identity;

namespace FavoriteMoviesFall2024.Server.Models;

public class ApplicationUser : IdentityUser
{
    public List<Movie> FavoriteMovies { get; set; } = new();
}
