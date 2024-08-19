using FavoriteMoviesFall2024.Client.Pages;
using FavoriteMoviesFall2024.Shared;
using FavoriteMoviesFall2024.Shared.Wrapper;
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
    
    public async Task<DataResponse<List<OMDBMovie>>> GetMovies()
    {
        var MovieDetails = new List<OMDBMovie>();
        DataResponse<UserDto> res = await _httpClient.GetFromJsonAsync<DataResponse<UserDto>>($"api/User");  // this is our server (FavoriteMoviesFall2024.Server Project)

        if (res.Succeeded)
        {
            UserDto User = res.Data;
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
        }

        
        return new DataResponse<List<OMDBMovie>>(MovieDetails);
    }

    public async Task<DataResponse<List<UserEditDto>>> GetUsers()
    {
        try
        {
            var res = await _httpClient.GetFromJsonAsync<DataResponse<List<UserEditDto>>>("api/users");
            if (res?.Succeeded ?? false)
            {
                return new DataResponse<List<UserEditDto>>(res.Data);
            } else
            {
                return new DataResponse<List<UserEditDto>>()
                {
                    Succeeded = false,
                    Message = "Users list not found",
                    Data = new List<UserEditDto>()
                };
            }
        } catch (Exception ex)
        {
            return new DataResponse<List<UserEditDto>>()
            {
                Succeeded = false,
                Message = ex.Message,
                Data = new List<UserEditDto>()
            };
        }
    }

    public async Task<Response> RemoveMovie(string imdbId)
    {
        try
        {
            Movie newMovie = new Movie { imdbId = imdbId };
            var res = await _httpClient.PostAsJsonAsync("api/remove-movie", newMovie);
            Response response = await res.Content.ReadFromJsonAsync<Response>();
            return response;
        } catch (Exception ex)
        {
            return new Response("Remove movie failed.");
        }
        

    }
}
