using FavoriteMoviesFall2024.Shared;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Notifications;
using Syncfusion.Blazor.Navigations;
using System.Net.Http.Json;

namespace FavoriteMoviesFall2024.Client.Pages;

public partial class Search
{
    [Inject]
    public HttpClient httpClient { get; set; } = new();
    [Inject]
    public AuthenticationStateProvider AuthenticationStateProvider { get; set; }
    private MovieSearchResult searchResults { get; set; } = new();
    private string searchTerm;
    private MovieSearchResultItem SelectedMovie;
    public SfToast ToastObj { get; set; } = null!;
    public string toastContent { get; set; } = String.Empty;
    public string toastCss = "e-toast-success";

    private List<MovieSearchResultItem> OMDBMovies { get; set; } = new();

    private readonly string OMDBAPIUrl = "https://www.omdbapi.com/?";
    private readonly string OMDBAPIKey = "apikey=86c39163";

    public async Task GetSelectedRows(RowSelectEventArgs<MovieSearchResultItem> args)
    {
        SelectedMovie = args.Data;
    }

    public async Task ToolbarClickHandler(ClickEventArgs args)
    {
        if (args.Item.Id == "GridMovieAdd")
        {
            await AddMovie();
        }
    }

    public async Task AddMovie()
    {
        Movie newMovie = new Movie
        {
            imdbId = SelectedMovie.imdbID
        };

        var UserAuth = (await AuthenticationStateProvider.GetAuthenticationStateAsync()).User.Identity;
        var res = await httpClient.PostAsJsonAsync($"api/add-movie?username={UserAuth.Name}", newMovie);
        if (res.IsSuccessStatusCode)
        {
            //toastContent = $"Movie {SelectedMovie.Title} added to your favorites.";
            //StateHasChanged();
            //await ToastObj.ShowAsync();
            toastService.ShowToast($"Movie {SelectedMovie.Title} added to your favorites.", Services.ToastLevel.Success, 5000);
            StateHasChanged();
        } else
        {
            toastService.ShowToast($"Error: {res.StatusCode}", Services.ToastLevel.Error, 5000);
            StateHasChanged();
        }
    }

    private async Task SearchOMDB()
    {
        searchResults = await httpClient.GetFromJsonAsync<MovieSearchResult>($"{OMDBAPIUrl}{OMDBAPIKey}&s={searchTerm}");
        if (searchResults?.Search?.Any() ?? false)
        {
            OMDBMovies = searchResults.Search.ToList();
        }
    }
}
