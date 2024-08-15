using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Duende.IdentityServer.EntityFramework.Options;
using FavoriteMoviesFall2024.Server.Models;
using FavoriteMoviesFall2024.Shared;
using Microsoft.AspNetCore.Identity;

namespace FavoriteMoviesFall2024.Server.Data;

public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>
{
    //private RoleManager<ApplicationUser> roleManager;

    public ApplicationDbContext(
        DbContextOptions options,
        //RoleManager<ApplicationUser> _roleManager,
        IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
    {
        //roleManager = _roleManager;
    }

    public DbSet<Movie> Movies => Set<Movie>();

    //protected override void OnModelCreating(ModelBuilder builder)
    //{
    //    //roleManager.CreateAsync("Admin");
    //    //User.Roles.Add("Admin");
    //}
}
