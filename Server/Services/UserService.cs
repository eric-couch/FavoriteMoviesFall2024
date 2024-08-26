using FavoriteMoviesFall2024.Shared;
using FavoriteMoviesFall2024.Server.Models;
using FavoriteMoviesFall2024.Server.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using FavoriteMoviesFall2024.Shared.Wrapper;
using System.Diagnostics;
using FavoriteMoviesFall2024.Server.Migrations;
using Microsoft.Extensions.Caching.Memory;

namespace FavoriteMoviesFall2024.Server.Services;

public class UserService : IUserService
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ILogger<UserService> _logger;
    private readonly HttpClient _httpClient;
    private readonly IMemoryCache _cache;

    public UserService( ApplicationDbContext context,
                        RoleManager<IdentityRole> roleManager,
                        UserManager<ApplicationUser> userManager,
                        ILogger<UserService> logger,
                        HttpClient httpClient,
                        IMemoryCache cache)
    {
        _context = context;
        _roleManager = roleManager;
        _userManager = userManager;
        _logger = logger;
        _httpClient = httpClient;
        _cache = cache;
    }

    private readonly string OMDBAPIUrl = "https://www.omdbapi.com/?";
    private readonly string OMDBAPIKey = "apikey=86c39163";
    private readonly string cacheKey = "OMDBMovies";

    public async Task<UserDto> GetMovies(string userName)
    {
        try
        {
            var user = await _userManager.FindByNameAsync(userName);

            if (user is null)
            {
                return new UserDto();
            }

            UserDto? movies = new();

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            if (_cache.TryGetValue(cacheKey, out IEnumerable<OMDBMovie> _movies)) {
                _logger.LogInformation("Retrieved movies from cache");
                movies.OMDBMovies = _movies.ToList();
            } else
            {
                // get user's info and favorite movies
                movies = await _context.Users.Include(u => u.FavoriteMovies)
                                .Select(u => new UserDto
                                {
                                    Id = u.Id,
                                    UserName = u.UserName,
                                    FavoriteMovies = u.FavoriteMovies
                                })
                                .FirstOrDefaultAsync(u => u.Id == user.Id);

                // get the movies that aren't the cache yet and add them to the cache
                var missingMovies = (from m in movies.FavoriteMovies
                                     where !_context.OMDBMovies.Any(omdb => omdb.imdbID == m.imdbId)
                                     select m).ToList();

                // add omdb movie cache for all missing movies
                foreach (var missingMovie in missingMovies)
                {
                    OMDBMovie omdbMovie = await _httpClient.GetFromJsonAsync<OMDBMovie>($"{OMDBAPIUrl}{OMDBAPIKey}&i={missingMovie.imdbId}");
                    if (omdbMovie is not null)
                    {
                        await _context.Ratings.AddRangeAsync(omdbMovie.Ratings);
                        await _context.OMDBMovies.AddAsync(omdbMovie);
                        await _context.SaveChangesAsync();
                    }
                }

                // grab all the omdb movies for the user and add it to the user dto
                var omdbMoviesForUser = await (from omdb in _context.OMDBMovies
                                               join m in _context.Movies on omdb.imdbID equals m.imdbId
                                               join u in _context.Users on m.ApplicationUserId equals u.Id
                                               where m.ApplicationUserId == user.Id
                                               select omdb)
                                               .Include(omdb => omdb.Ratings)
                                               .ToListAsync();

                movies.OMDBMovies = omdbMoviesForUser;

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromSeconds(60))
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(60))
                    .SetPriority(CacheItemPriority.Normal);

                // add movies to cache
                _cache.Set(cacheKey, omdbMoviesForUser, cacheEntryOptions);

            }

            

            stopwatch.Stop();
            _logger.LogInformation("Elapsed Time retrieving movies: {ElapsedMilliseconds} ms", stopwatch.ElapsedMilliseconds);
            // api calls took       Elapsed Time retrieving movies: 1718 ms
            // from db took         Elapsed Time retrieving movies: 1219 ms
            // from db second pass  Elapsed Time retrieving movies: 1262 ms
            // from cache           Elapsed Time retrieving movies: 2 ms    

            return movies;
        } catch (Exception e)
        {
            Debug.WriteLine(e.Message);
            _logger.LogError(e, "Error retrieving movies");
            return new UserDto();
        }

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

            _logger.LogInformation("Retrieved {Count} users. Logged at {Placeholder:MMMM dd, yyyy}", users.Count, DateTimeOffset.UtcNow);
            // Trace = 0, Debug = 1, Information = 2, Warning = 3, Error = 4, Critical = 5, None = 6

            return new List<UserEditDto>(users);
        }
        catch (Exception e)
        {
            Debug.WriteLine(e.Message);
            _logger.LogError(e, "Error retrieving users");
            return new List<UserEditDto>();
        }
    }
}
