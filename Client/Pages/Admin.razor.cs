using FavoriteMoviesFall2024.Shared;
using FavoriteMoviesFall2024.Shared.Wrapper;
using FavoriteMoviesFall2024.Client.HttpRepo;
using Syncfusion.Blazor.Grids;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;

namespace FavoriteMoviesFall2024.Client.Pages;

public partial class Admin
{
    [Inject]
    public IUserMoviesHttpRepository UserMoviesRepo { get; set; }
    public List<UserEditDto> Users { get; set; } = new List<UserEditDto>();
    public SfGrid<UserEditDto> Grid { get; set; }
    public string toastCss = "e-toast-success";

    protected override async Task OnInitializedAsync()
    {
        var res = await UserMoviesRepo.GetUsers();

        if (res.Succeeded)
        {
            Users = res.Data;
        }
        else
        {
            toastCss = "e-toast-danger";
            toastService.ShowToast($"Error retrieving users: {res.Message}.", Services.ToastLevel.Success, 5000);
        }
    }

    public async Task ToggleEnabledUser(ChangeEventArgs args, string UserId)
    {
        Response res = await UserMoviesRepo.ToggleEnabledUser(UserId);
        if (!res.Succeeded)
        {
            toastCss = "e-toast-danger";
            toastService.ShowToast($"Error toggling enable user: {res.Message}.", Services.ToastLevel.Success, 5000);
        }
    }
    public async Task ToggleAdminRole(ChangeEventArgs args, string UserId)
    {
        Response res = await UserMoviesRepo.ToggleAdminRole(UserId);
        if (!res.Succeeded)
        {
            toastCss = "e-toast-danger";
            toastService.ShowToast($"Error toggling admin role: {res.Message}.", Services.ToastLevel.Success, 5000);
        }
    }

}
