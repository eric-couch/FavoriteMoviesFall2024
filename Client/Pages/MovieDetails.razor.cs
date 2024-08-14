using FavoriteMoviesFall2024.Shared;
using Microsoft.AspNetCore.Components;

namespace FavoriteMoviesFall2024.Client.Pages;

public partial class MovieDetails
{
    [Parameter]
    public OMDBMovie? Movie { get; set; }
    [Parameter]
    public bool AllowDelete { get; set; }
    [Parameter]
    public EventCallback<OMDBMovie> OnRemoveFavoriteMovie { get; set; }

    public async Task RemoveFavoriteMovie(OMDBMovie movie)
    {
        await OnRemoveFavoriteMovie.InvokeAsync(movie);
    }
}
