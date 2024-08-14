using FavoriteMoviesFall2024.Shared;

namespace FavoriteMoviesFall2024.Client.HttpRepo;

public interface IUserMoviesHttpRepository
{
    Task<List<OMDBMovie>> GetMovies();
    Task<bool> RemoveMovie(string imdbId);
}
