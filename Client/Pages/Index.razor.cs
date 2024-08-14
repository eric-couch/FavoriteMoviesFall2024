using FavoriteMoviesFall2024.Shared;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using FavoriteMoviesFall2024.Client.HttpRepo;

namespace FavoriteMoviesFall2024.Client.Pages;

public partial class Index
{
    [Inject]
    public IUserMoviesHttpRepository UserMoviesHttpRepository { get; set; }
    [Inject]
    public AuthenticationStateProvider AuthenticationStateProvider { get; set; }

    public UserDto User { get; set; } = new();
    public List<OMDBMovie> MovieDetails { get; set; } = new();

    protected override async Task OnInitializedAsync()
    {
        var UserAuth = (await AuthenticationStateProvider.GetAuthenticationStateAsync()).User.Identity;

        if (UserAuth is not null && UserAuth.IsAuthenticated)
        {
            MovieDetails = await UserMoviesHttpRepository.GetMovies();
        }
    }

    private async Task RemoveFavoriteMovie(OMDBMovie movie)
    {
        bool res = await UserMoviesHttpRepository.RemoveMovie(movie.imdbID);
        if (res)
        {
            MovieDetails.Remove(movie);
            StateHasChanged();
            toastService.ShowToast($"Removed movie {movie.Title} sucessfully.", Services.ToastLevel.Success, 5000);
            StateHasChanged();
        } else
        {
            toastService.ShowToast($"Failed to remove movie {movie.Title}.", Services.ToastLevel.Error, 5000);
            StateHasChanged();
        }
    }
}
