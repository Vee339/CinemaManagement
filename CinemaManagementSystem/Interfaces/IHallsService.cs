using CinemaManagementSystem.Models;

namespace CinemaManagementSystem.Interfaces
{
    public interface IHallsService
    {
        Task<IEnumerable<Hall>> GetHalls();

        Task<Hall> FindHall(int id);

        Task AddHall(Hall hall);

        Task<string> EditHall(int id, Hall hall);

        Task<string> DeleteHall(int id);

        Task<IEnumerable<Hall>> ListHallsForMovie(int id);
    }
}
