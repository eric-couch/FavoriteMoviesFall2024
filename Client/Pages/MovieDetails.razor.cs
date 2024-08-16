using FavoriteMoviesFall2024.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace FavoriteMoviesFall2024.Client.Pages;

public partial class MovieDetails
{
    [Inject]
    public AuthenticationStateProvider AuthenticationStateProvider { get; set; }
    [Parameter]
    public OMDBMovie? Movie { get; set; }
    [Parameter]
    public bool AllowDelete { get; set; }
    [Parameter]
    public EventCallback<OMDBMovie> OnRemoveFavoriteMovie { get; set; }

    private bool ShowDelete { get; set; } = false;

    protected override async Task OnInitializedAsync()
    {
        var UserAuth = (await AuthenticationStateProvider.GetAuthenticationStateAsync()).User;

        if (UserAuth.IsInRole("Admin"))
        {
            ShowDelete = true;
        }

    }

    public async Task RemoveFavoriteMovie(OMDBMovie movie)
    {
        await OnRemoveFavoriteMovie.InvokeAsync(movie);
    }
}
