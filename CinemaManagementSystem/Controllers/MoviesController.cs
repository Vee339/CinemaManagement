using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CinemaManagementSystem.Data;
using CinemaManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using System.Numerics;
using System.Linq;

namespace CinemaManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MoviesController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// This API gives the List of all the movies in the database. 
        /// </summary>
        /// <example>
        /// GET: api/Movies/GetMovies -> 
        /// [{"movieId":1,"movieName":"The Intern","language":"English","totalScreenings":1},{"movieId":2,"movieName":"Avengers","language":"English","totalScreenings":1},{"movieId":5,"movieName":"PK","language":"Hindi","totalScreenings":1},{"movieId":9,"movieName":"The Dark Night","language":"English","totalScreenings":1},{"movieId":10,"movieName":"Parasite","language":"English","totalScreenings":1},{"movieId":12,"movieName":"Raazi","language":"Hindi","totalScreenings":1}]
        /// </example>
        /// <returns>
        /// The List of Movie Dtos. A Dto include MovieId, MovieName, Language, and Total Screenings.
        /// </returns>
        [HttpGet(template: "GetMovies")]
        public async Task<ActionResult<IEnumerable<MovieDto>>> GetMovies()
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

        /// <summary>
        /// This api endpoint finds a movie when the id of the movie is provided.
        /// </summary>
        /// <param name="id">The id of numeric type of the movie that user wants to get</param>
        /// <example>
        /// GET: api/Movies/FindMovie/9 -> {"movieId":9,"movieName":"The Dark Night","language":"English","totalScreenings":1}
        /// </example>
        /// <returns>
        /// Returns the Movie Dto which includes MovieId, MovieName, Language, and Total Screenings.
        /// </returns>

        [HttpGet(template: "FindMovie/{id}")]

        public async Task<ActionResult<MovieDto>> FindMovie(int id)
        {
            MovieDto MovieDto = new MovieDto();

            Movie Movie = await _context.Movies.Include(m => m.Screenings).Where(m => m.MovieId == id).FirstOrDefaultAsync();

            MovieDto.MovieId = Movie.MovieId;
            MovieDto.MovieName = Movie.Title;
            MovieDto.Language = Movie.Language;
            MovieDto.TotalScreenings = Movie.Screenings.Count;

            return MovieDto;
        }


        /// <summary>
        /// This api endpoint receives the information of a movie and insert that into the database.
        /// </summary>
        /// <example>
        /// 
        /// POST
        /// 
        /// Header: 
        /// Accept: text/plain
        /// Content-type: application/json
        /// 
        /// Request body: 
        ///{"Title":"Baby John","Genre":"Rom Com","Language":"English","ReleaseDate":"2024-12-08","Duration":78}
        ///
        /// </example>
        /// <returns>
        /// Returns the movie that has just been added.
        /// </returns>
        [HttpPost(template: "AddMovie")]

        public async Task<ActionResult<Movie>> AddMovie(Movie movie)
        {
            _context.Movies.Add(movie);

            await _context.SaveChangesAsync();

            return CreatedAtAction("FindMovie", new { id = movie.MovieId }, movie);
        }

        /// <summary>
        /// This endpoint updates a movie in the database.
        /// </summary>
        /// <param name="id">The id of the movie that the user wants to update.</param>
        /// <param name="movie">The movie data that user wants to change.</param>
        /// <example>
        /// PUT: api/Movie/PutMovie/10
        /// Headers: Content-Type: application/json
        /// Request Body:
        /// {
        ///     "MovieId":10,
        ///     "Title":"A Wednesday",
        ///     "Genre":"Horror",
        ///     "Language":"Hindi",
        ///     "ReleaseDate":"23-02-2009",
        ///     "Duration":105
        /// }
        /// </example>
        /// <returns>
        /// Movie is successfully updated -> No Content
        /// Movie with the provided id does not exist -> Not Found
        /// The id of the movie does not match the id parameter -> Bad Request
        /// </returns>
        [HttpPut("{id}")]

        public async Task<IActionResult> PutMovie(int id, Movie movie)
        {
            if(id != movie.MovieId)
            {
                return BadRequest();
            }
            
            _context.Entry(movie).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException)
            {
                if (!MovieExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            

            return NoContent();
        }

        /// <summary>
        /// This api endpoints deletes a movie from the database with the help of id.
        /// </summary>
        /// <param name="id">The id of the movie as an int that user wants to delete.</param>
        /// <example>
        /// 
        /// DELETE: api/Movies/DeleteMovie/10 -> No Content
        /// 
        /// </example>
        /// <returns>
        /// Movie is successfully deleted -> No Content
        /// Movie with the provided id does not exist -> Not Found 
        /// </returns>

        [HttpDelete(template: "DeleteMovie/{id}")]
        public async Task<ActionResult> DeleteMovie(int id)
        {
            var movie = await _context.Movies.FindAsync(id);

            if (movie == null)
            {
                return NotFound();
            }

            _context.Movies.Remove(movie);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET - api/Movies/ListMoviesForHall/3

        /// <summary>
        /// This api endpoint returns the List of Movies that are being screened in a Hall
        /// </summary>
        /// <param name="id">The id of the hall in which the user wants to find the movies.</param>
        /// <example>
        /// GET: api/Movies/ListMoviesForHall/4 -> [{"movieId":9,"title":"The Dark Night","genre":"Thriller","language":"English","releaseDate":"2008-12-07T00:00:00","duration":95,"screenings":null,"halls":null},{"movieId":5,"title":"PK","genre":"comedy","language":"Hindi","releaseDate":"2014-03-14T00:00:00","duration":92,"screenings":null,"halls":null},{"movieId":1,"title":"The Intern","genre":"business","language":"English","releaseDate":"2025-04-13T00:00:00","duration":214,"screenings":null,"halls":null}]
        /// </example>
        /// <returns>
        /// The List of the movies in the hall of which id is given.
        /// </returns>

        [HttpGet(template:"ListMoviesForHall/{id}")]
        public async Task<ActionResult<IEnumerable<Movie>>> ListMoviesForHall(int id)
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
