using FavoriteMoviesFall2024.Client.Pages;
using FavoriteMoviesFall2024.Shared;
using Microsoft.AspNetCore.Components.Authorization;
using Syncfusion.Blazor.PivotView;
using System.Net.Http;
using System.Net.Http.Json;

namespace FavoriteMoviesFall2024.Client.HttpRepo;

public class UserMoviesHttpRepository : IUserMoviesHttpRepository
{
    public readonly HttpClient _httpClient;
    private readonly string OMDBAPIUrl = "https://www.omdbapi.com/?";
    private readonly string OMDBAPIKey = "apikey=86c39163";

    public UserMoviesHttpRepository(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<OMDBMovie>> GetMovies()
    {
        var MovieDetails = new List<OMDBMovie>();
        UserDto User = await _httpClient.GetFromJsonAsync<UserDto>($"api/User");  // this is our server (FavoriteMoviesFall2024.Server Project)

        if (User?.FavoriteMovies?.Any() ?? false)
        {
            foreach (var movie in User.FavoriteMovies)
            {
                // OMDB API is a microservice
                OMDBMovie omdbMovie = await _httpClient.GetFromJsonAsync<OMDBMovie>($"{OMDBAPIUrl}{OMDBAPIKey}&i={movie.imdbId}");
                if (omdbMovie is not null)
                {
                    MovieDetails.Add(omdbMovie);
                }
            }
        }
        return MovieDetails;
    }
    public async Task<bool> RemoveMovie(string imdbId)
    {
        
        Movie newMovie = new Movie { imdbId = imdbId };
        var res = await _httpClient.PostAsJsonAsync("api/remove-movie", newMovie);
        if (res.StatusCode == System.Net.HttpStatusCode.Found)
        {
            return false;
        }

        if (!res.IsSuccessStatusCode)
        {
            return false;
        }
        return true;
    }
}
