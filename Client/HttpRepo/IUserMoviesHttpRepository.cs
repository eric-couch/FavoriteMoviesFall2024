using FavoriteMoviesFall2024.Shared;
using FavoriteMoviesFall2024.Shared.Wrapper;

namespace FavoriteMoviesFall2024.Client.HttpRepo;

public interface IUserMoviesHttpRepository
{
    Task<DataResponse<List<OMDBMovie>>> GetMovies();
    Task<Response> RemoveMovie(string imdbId);
    Task<DataResponse<List<UserEditDto>>> GetUsers();
}
