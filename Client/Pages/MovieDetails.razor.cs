using FavoriteMoviesFall2024.Shared;
using Microsoft.AspNetCore.Components;

namespace FavoriteMoviesFall2024.Client.Pages;

public partial class MovieDetails
{
    [Parameter]
    public OMDBMovie? Movie { get; set; }
}
