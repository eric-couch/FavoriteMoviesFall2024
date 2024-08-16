using FavoriteMoviesFall2024.Shared;
using FavoriteMoviesFall2024.Shared.Wrapper;
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
            var res = await UserMoviesHttpRepository.GetMovies();
            if (res.Succeeded)
            {
                MovieDetails = res.Data;
            } else
            {
                // show toast what went wrong!!!
            }

        }
    }

    private async Task RemoveFavoriteMovie(OMDBMovie movie)
    {
        Response res = await UserMoviesHttpRepository.RemoveMovie(movie.imdbID);
        if (res.Succeeded)
        {
            MovieDetails.Remove(movie);
            StateHasChanged();
            toastService.ShowToast($"Removed movie {movie.Title} sucessfully.", Services.ToastLevel.Success, 5000);
            StateHasChanged();
        } else
        {
            toastService.ShowToast($"{res.Message}", Services.ToastLevel.Error, 5000);
            StateHasChanged();
        }
    }
}
