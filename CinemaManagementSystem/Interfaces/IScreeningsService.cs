using CinemaManagementSystem.Models;

namespace CinemaManagementSystem.Interfaces
{
    public interface IScreeningsService
    {
        Task<IEnumerable<ScreeningDto>> GetScreenings();

        Task<ScreeningDto> FindScreening(int id);

        Task<string> AddScreening(Screening screening);

        Task<string> EditScreening(int id, Screening screening);

        Task<string> DeleteScreening(int id);

        Task<IEnumerable<ScreeningDto>> ListScreeningsForMovie(int id);

        Task<IEnumerable<ScreeningDto>> ListScreeningsForHall(int id);

        Task<string> RemoveScreeningsForMovie(int id);

        Task<string> RemoveScreeningsForHall(int id);
    }
}
