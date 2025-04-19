using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CinemaManagementSystem.Models;
using CinemaManagementSystem.Data;
using CinemaManagementSystem.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CinemaManagementSystem.Services
{
    public class ScreeningsService : IScreeningsService
    {
        private readonly ApplicationDbContext _context;

        public ScreeningsService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ScreeningDto>> GetScreenings()
        {
            List<Screening> Screenings = await _context.Screenings.Include(s => s.Movie).Include(s => s.Hall).ToListAsync();

            List<ScreeningDto> ScreeningDtos = new List<ScreeningDto>();

            foreach (Screening Screening in Screenings)
            {
                ScreeningDto ScreeningDto = new ScreeningDto();

                ScreeningDto.ScreeningId = Screening.ScreeningId;
                ScreeningDto.Movie = Screening.Movie.Title;
                ScreeningDto.Hall = Screening.Hall.Name;
                ScreeningDto.ShowDate = Screening.ScreeningDate;
                ScreeningDto.StartTime = Screening.StartTime;
                ScreeningDto.EndTime = Screening.EndTime;

                ScreeningDtos.Add(ScreeningDto);

            }

            return ScreeningDtos;
        }

        public async Task<ScreeningDto> FindScreening(int id)
        {
            Screening Screening = await _context.Screenings.Include(s => s.Movie).Include(s => s.Hall).Where(s => s.ScreeningId == id).FirstOrDefaultAsync();

            ScreeningDto ScreeningDto = new ScreeningDto();

            ScreeningDto.ScreeningId = Screening.ScreeningId;
            ScreeningDto.Movie = Screening.Movie.Title;
            ScreeningDto.Hall = Screening.Hall.Name;
            ScreeningDto.ShowDate = Screening.ScreeningDate;
            ScreeningDto.StartTime = Screening.StartTime;
            ScreeningDto.EndTime = Screening.EndTime;

            return ScreeningDto;
        }

        public async Task<string> AddScreening(Screening screening)
        {
            _context.Screenings.Add(screening);

            await _context.SaveChangesAsync();

            return "Screening is added successfully";

        }

        public async Task<string> EditScreening(int id, Screening screening)
        {
            if (id != screening.ScreeningId)
            {
                return "Bad Request";
            }

            _context.Entry(screening).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ScreeningExists(id))
                {
                    return "Not Found";
                }
                else
                {
                    throw;
                }

            }

            return "No Content";
        }

        public async Task<string> DeleteScreening(int id)
        {
            var screening = await _context.Screenings.FindAsync(id);

            if (screening == null)
            {
                return "Not Found";
            }

            _context.Screenings.Remove(screening);

            await _context.SaveChangesAsync();

            return "No Content";
        }

        public async Task<IEnumerable<ScreeningDto>> ListScreeningsForMovie(int id)
        {
            List<Screening> Screenings = await _context.Screenings.Include(s => s.Movie).Include(s => s.Hall).Where(s => s.MovieId == id).ToListAsync();

            List<ScreeningDto> ScreeningDtos = new List<ScreeningDto>();

            foreach (Screening Screening in Screenings)
            {
                ScreeningDto ScreeningDto = new ScreeningDto();

                ScreeningDto.ScreeningId = Screening.ScreeningId;
                ScreeningDto.Movie = Screening.Movie.Title;
                ScreeningDto.Hall = Screening.Hall.Name + " " + Screening.Hall.Location;
                ScreeningDto.ShowDate = Screening.ScreeningDate;
                ScreeningDto.StartTime = Screening.StartTime;
                ScreeningDto.EndTime = Screening.EndTime;

                ScreeningDtos.Add(ScreeningDto);
            }

            return ScreeningDtos;
        }

        public async Task<IEnumerable<ScreeningDto>> ListScreeningsForHall(int id)
        {
            List<Screening> Screenings = await _context.Screenings.Include(s => s.Movie).Include(s => s.Hall).Where(s => s.HallId == id).ToListAsync();

            List<ScreeningDto> ScreeningDtos = new List<ScreeningDto>();

            foreach (Screening Screening in Screenings)
            {
                ScreeningDto ScreeningDto = new ScreeningDto();

                ScreeningDto.ScreeningId = Screening.ScreeningId;
                ScreeningDto.Movie = Screening.Movie.Title;
                ScreeningDto.Hall = Screening.Hall.Name + " " + Screening.Hall.Location;
                ScreeningDto.ShowDate = Screening.ScreeningDate;
                ScreeningDto.StartTime = Screening.StartTime;
                ScreeningDto.EndTime = Screening.EndTime;

                ScreeningDtos.Add(ScreeningDto);
            }

            return ScreeningDtos;
        }

        public async Task<string> RemoveScreeningsForMovie(int id)
        {
            var screenings = await _context.Screenings.Where(s => s.MovieId == id).ToListAsync();
            if (screenings == null)
            {
                return "Not Found";
            }
            foreach (var screening in screenings)
            {
                _context.Screenings.Remove(screening);

                await _context.SaveChangesAsync();
            }

            return "No Content";
        }

        public async Task<string> RemoveScreeningsForHall(int id)
        {
            var screenings = await _context.Screenings.Where(s => s.HallId == id).ToListAsync();

            if (screenings == null)
            {
                return "Not Found";
            }

            foreach (var screening in screenings)
            {
                _context.Screenings.Remove(screening);

                await _context.SaveChangesAsync();
            }

            return "No Content";
        }
        private bool ScreeningExists(int id)
        {
            return _context.Screenings.Any(e => e.ScreeningId == id);
        }
    }
}
