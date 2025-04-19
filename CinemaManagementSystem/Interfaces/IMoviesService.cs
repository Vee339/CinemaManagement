using CinemaManagementSystem.Models;

namespace CinemaManagementSystem.Interfaces
{
    public interface IMoviesService
    {
        Task<IEnumerable<MovieDto>> GetMovies();

        Task<MovieDto> FindMovie(int id);

        Task<Movie> FindMovieDetails(int id);

        Task AddMovie(Movie movie);

        Task<string> PutMovie(int id, Movie movie);

        Task<string> DeleteMovie(int id);
        
        Task<IEnumerable<Movie>> ListMoviesForHall(int id);
    }
}
