using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CinemaManagementSystem.Interfaces;
using CinemaManagementSystem.Models;
using CinemaManagementSystem.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Numerics;
using System.Linq;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CinemaManagementSystem.Services
{
    public class MoviesService : IMoviesService
    {
        private readonly ApplicationDbContext _context;

        public MoviesService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MovieDto>> GetMovies()
        {
            List<Movie> Movies = await _context.Movies.Include(m => m.Screenings).ToListAsync();

            List<MovieDto> MovieDtos = new List<MovieDto>();

            foreach (Movie Movie in Movies)
            {
                MovieDto MovieDto = new MovieDto();
                MovieDto.MovieId = Movie.MovieId;
                MovieDto.MovieName = Movie.Title;
                MovieDto.Language = Movie.Language;
                MovieDto.TotalScreenings = Movie.Screenings.Count();

                MovieDtos.Add(MovieDto);
            }
            return MovieDtos;
        }

        public async Task<MovieDto> FindMovie(int id)
        {
            MovieDto MovieDto = new MovieDto();

            Movie Movie = await _context.Movies.Include(m => m.Screenings).Where(m => m.MovieId == id).FirstOrDefaultAsync();

            MovieDto.MovieId = Movie.MovieId;
            MovieDto.MovieName = Movie.Title;
            MovieDto.Language = Movie.Language;
            MovieDto.TotalScreenings = Movie.Screenings.Count;

            return MovieDto;
        }

        public async Task<Movie> FindMovieDetails(int id)
        {
            Movie Movie = await _context.Movies.Include(m => m.Screenings).Where(m => m.MovieId == id).FirstOrDefaultAsync();

            return Movie;
        }

        public async Task AddMovie(Movie movie)
        {
            _context.Movies.Add(movie);

            await _context.SaveChangesAsync();

           
        }

        public async Task<string> PutMovie(int id, Movie movie)
        {
            if (id != movie.MovieId)
            {
                return "Bad Request";
            }

            _context.Entry(movie).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieExists(id))
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

        public async Task<string> DeleteMovie(int id)
        {
            var movie = await _context.Movies.FindAsync(id);

            if (movie == null)
            {
                return "Not Found";
            }

            _context.Movies.Remove(movie);

            await _context.SaveChangesAsync();

            return "No Content";
        }

        public async Task<IEnumerable<Movie>> ListMoviesForHall(int id)
        {

            List<Movie> Movies = await _context.Movies.Join(_context.Screenings, Movie => Movie.MovieId, Screening => Screening.MovieId, (Movie, Screening) => new { Movie, Screening }).Where(ms => ms.Screening.HallId == id).Select(ms => ms.Movie).ToListAsync();

            return Movies;
        }

        private bool MovieExists(int id)
        {
            return _context.Movies.Any(e => e.MovieId == id);
        }
    }
}
