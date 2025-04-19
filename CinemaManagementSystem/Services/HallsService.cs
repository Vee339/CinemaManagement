using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CinemaManagementSystem.Data;
using CinemaManagementSystem.Models;
using CinemaManagementSystem.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CinemaManagementSystem.Services
{
    public class HallsService : IHallsService
    {
        private readonly ApplicationDbContext _context;

        public HallsService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Hall>> GetHalls()
        {
            return await _context.Halls.ToListAsync();
        }


        public async Task<Hall> FindHall(int id)
        {
            return await _context.Halls.FindAsync(id);
        }

        public async Task AddHall(Hall hall)
        {
            _context.Halls.Add(hall);

            await _context.SaveChangesAsync();

        }

        public async Task<string> EditHall(int id, Hall hall)
        {
            if (id != hall.HallId)
            {
                return "Bad Request";
            }

            _context.Entry(hall).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HallExists(id))
                {
                    return "Bad Request";
                }
                else
                {
                    throw;
                }
            }

            return "No Content";
        }

        public async Task<string> DeleteHall(int id)
        {
            var hall = await _context.Halls.FindAsync(id);

            if (hall == null)
            {
                return "Not Found";
            }

            _context.Halls.Remove(hall);

            await _context.SaveChangesAsync();

            return "No Content";
        }

        public async Task<IEnumerable<Hall>> ListHallsForMovie(int id)
        {
            List<Hall> Halls = await _context.Halls.Join(_context.Screenings, Hall => Hall.HallId, Screening => Screening.HallId, (Hall, Screening) => new { Hall, Screening }).Where(hs => hs.Screening.MovieId == id).Select(hs => hs.Hall).ToListAsync();

            return Halls;
        }
        private bool HallExists(int id)
        {
            return _context.Halls.Any(e => e.HallId == id);
        }
    }
}
