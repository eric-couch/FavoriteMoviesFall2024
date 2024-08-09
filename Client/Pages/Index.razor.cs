using FavoriteMoviesFall2024.Shared;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace FavoriteMoviesFall2024.Client.Pages;

public partial class Index
{
    [Inject]
    public HttpClient httpClient { get; set; } = new();
    [Inject]
    public AuthenticationStateProvider AuthenticationStateProvider { get; set; }

    public UserDto User { get; set; } = new();
    public List<OMDBMovie> MovieDetails { get; set; } = new();

    private readonly string OMDBAPIUrl = "https://www.omdbapi.com/?";
    private readonly string OMDBAPIKey = "apikey=86c39163";

    protected override async Task OnInitializedAsync()
    {
        var UserAuth = (await AuthenticationStateProvider.GetAuthenticationStateAsync()).User.Identity;

        if (UserAuth is not null && UserAuth.IsAuthenticated)
        {
            User = await httpClient.GetFromJsonAsync<UserDto>($"api/User?userId={UserAuth.Name}");  // this is our server (FavoriteMoviesFall2024.Server Project)

            if (User?.FavoriteMovies?.Any() ?? false)
            {
                foreach (var movie in User.FavoriteMovies)
                {
                    // OMDB API is a microservice
                    OMDBMovie omdbMovie = await httpClient.GetFromJsonAsync<OMDBMovie>($"{OMDBAPIUrl}{OMDBAPIKey}&i={movie.imdbId}");
                    if (omdbMovie is not null)
                    {
                        MovieDetails.Add(omdbMovie);
                    }
                }
            }
        }
    }
}
