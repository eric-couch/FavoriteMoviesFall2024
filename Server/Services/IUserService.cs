using FavoriteMoviesFall2024.Shared;
using FavoriteMoviesFall2024.Shared.Wrapper;

namespace FavoriteMoviesFall2024.Server.Services;

public interface IUserService
{
    Task<UserDto> GetMovies(string userName);
    Task<List<UserEditDto>> GetUsers();
}
