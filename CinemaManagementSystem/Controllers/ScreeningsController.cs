using CinemaManagementSystem.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CinemaManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace CinemaManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScreeningsController : ControllerBase
    {
       private readonly ApplicationDbContext _context;

       public ScreeningsController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// This api endpoint gives all the screenings in the database.
        /// </summary>
        /// <example>
        /// GET: api/Screenings/GetScreenings: -> 
        /// 
        /// [{"screeningId":6,"movie":"The Dark Night","hall":"s2e7 2","showDate":"2025-02-19","startTime":"17:45:23","endTime":"19:55:00"},{"screeningId":9,"movie":"PK","hall":"s2e7 2","showDate":"2021-07-15","startTime":"15:45:00","endTime":"18:00:00"},{"screeningId":10,"movie":"Raazi","hall":"x3j5 6","showDate":"2020-05-26","startTime":"19:40:15","endTime":"21:30:00"},{"screeningId":11,"movie":"Avengers","hall":"k4b7 5","showDate":"2019-10-16","startTime":"15:45:35","endTime":"18:15:00"},{"screeningId":12,"movie":"Parasite","hall":"x3j5 6","showDate":"2022-08-19","startTime":"21:45:00","endTime":"23:00:00"},{"screeningId":13,"movie":"The Intern","hall":"s2e7 2","showDate":"2023-07-18","startTime":"20:15:00","endTime":"22:10:00"}]
        /// 
        /// </example>
        /// <returns>
        /// The List of the screenings
        /// </returns>
        [HttpGet(template:"GetScreenings")]

        public async Task<ActionResult<IEnumerable<ScreeningDto>>> GetScreenings()
        {
            List<Screening> Screenings = await _context.Screenings.Include(s => s.Movie).Include(s => s.Hall).ToListAsync();

            List<ScreeningDto> ScreeningDtos = new List<ScreeningDto>();

            foreach(Screening Screening in Screenings)
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

        /// <summary>
        /// This api endpoint finds a screening with an id 
        /// </summary>
        /// <param name="id">The id of the screening that user wants to find.</param>
        /// <example>
        /// 
        /// GET: api/Screenings/FindScreening/5 -> {"screeningId":12,"movie":"Parasite","hall":"x3j5 6","showDate":"2022-08-19","startTime":"21:45:00","endTime":"23:00:00"}
        ///
        /// </example>
        /// <returns>
        /// Returns the single screening associated with the id provided.
        /// </returns>
        [HttpGet(template:"FindScreening/{id}")]

        public async Task<ActionResult<ScreeningDto>> FindScreening(int id)
        {
            Screening Screening = await _context.Screenings.Include(s => s.Movie).Include(s => s.Hall).Where(s => s.ScreeningId == id).FirstOrDefaultAsync();

            ScreeningDto ScreeningDto = new ScreeningDto();

            ScreeningDto.ScreeningId = Screening.ScreeningId;
            ScreeningDto.Movie = Screening.Movie.Title;
            ScreeningDto.Hall = Screening.Hall.Name + " " + Screening.Hall.Location;
            ScreeningDto.ShowDate = Screening.ScreeningDate;
            ScreeningDto.StartTime = Screening.StartTime;
            ScreeningDto.EndTime = Screening.EndTime;

            return ScreeningDto;
        }


        /// <summary>
        /// This api endpoint receives the information of a screening and insert that into the database.
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
        ///{"MovieId":13,"HallId":5,"ScreeningDate":"2023-07-13","StartTime":"15:45:00","EndTime":"19:00:00"}
        ///
        /// </example>
        /// <returns>
        /// String - "Screening is added successfully".
        /// </returns>
        [HttpPost(template:"AddScreening")]
        public async Task<string> AddScreening(Screening screening)
        {
            _context.Screenings.Add(screening);

            await _context.SaveChangesAsync();

            return "Screening is added successfully";

        }


        /// <summary>
        /// This endpoint updates a screening in the database.
        /// </summary>
        /// <param name="id">The id of the screening that the user wants to update.</param>
        /// <param name="Screening">The hall data that user wants to change.</param>
        /// <example>
        /// PUT: api/Screening/EditScreening/3
        /// Headers: Content-Type: application/json
        /// Request Body:
        /// {
        ///     "ScreeningId":14,
        ///     "MovieId":5,
        ///     "HallId":3,
        ///     "ScreeningDate":"02-05-2023",
        ///     "StartTime":"18:20:00",
        ///     "EndTime":"20:30:00"
        /// }
        /// </example>
        /// <returns>
        /// Screening is successfully updated -> No Content
        /// Screening with the provided id does not exist -> Not Found
        /// The id of the screening does not match the id parameter -> Bad Request
        /// </returns>

        [HttpPut("{id}")]
        public async Task <IActionResult> EditScreening(int id, Screening screening)
        {
            if(id != screening.ScreeningId)
            {
                return BadRequest();
            }

            _context.Entry(screening).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DBConcurrencyException)
            {
                if (!ScreeningExists(id))
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
        /// This api endpoints deletes a screening from the database with the help of id.
        /// </summary>
        /// <param name="id">The id (data-type = int) of the screening that user wants to delete.</param>
        /// <example>
        /// 
        /// DELETE: api/Screening/DeleteScreening/10 -> No Content
        /// 
        /// </example>
        /// <returns>
        /// Screening is successfully deleted -> No Content
        /// Screening with the provided id does not exist -> Not Found 
        /// </returns>

        [HttpDelete(template: "DeleteScreening/{id}")]
        public async Task<ActionResult> DeleteScreening(int id)
        {
            var screening = await _context.Screenings.FindAsync(id);

            if (screening == null)
            {
                return NotFound();
            }

            _context.Screenings.Remove(screening);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// This api endpoint gives the list of screenings for a particular movie
        /// </summary>
        /// <param name="id">The Id of the movie for which user wants to get the screenings.</param>
        /// <example>
        /// GET: api/Screenings/ListScreeningsForMovie/10 -> [{"screeningId":12,"movie":"Parasite","hall":"x3j5 6","showDate":"2022-08-19","startTime":"21:45:00","endTime":"23:00:00"}]
        /// </example>
        /// <returns>
        /// Returns the list of the screenings for the movie of which user gave the id
        /// </returns>

        [HttpGet(template: "ListScreeningsForMovie/{id}")]
        public async Task<ActionResult<IEnumerable<ScreeningDto>>> ListScreeningsForMovie(int id)
        {
            List<Screening> Screenings = await _context.Screenings.Include(s => s.Movie).Include(s => s.Hall).Where(s => s.MovieId == id).ToListAsync();

            List<ScreeningDto> ScreeningDtos = new List<ScreeningDto>();

            foreach(Screening Screening in Screenings)
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

        /// <summary>
        /// This api endpoint gives the list of screenings that are taking place in a hall
        /// </summary>
        /// <param name="id">The Id of the hall for which user wants to get the screenings.</param>
        /// <example>
        /// GET: api/Screenings/ListScreeningsForHall/4 -> [{"screeningId":6,"movie":"The Dark Night","hall":"s2e7 2","showDate":"2025-02-19","startTime":"17:45:23","endTime":"19:55:00"},{"screeningId":9,"movie":"PK","hall":"s2e7 2","showDate":"2021-07-15","startTime":"15:45:00","endTime":"18:00:00"},{"screeningId":13,"movie":"The Intern","hall":"s2e7 2","showDate":"2023-07-18","startTime":"20:15:00","endTime":"22:10:00"}]
        /// </example>
        /// <returns>
        /// Returns the list of the screenings for the hall of which user gave the id
        /// </returns>

        [HttpGet(template: "ListScreeningsForHall/{id}")]

        public async Task<ActionResult<IEnumerable<ScreeningDto>>> ListScreeningsForHall(int id)
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

        /// <summary>
        /// This api endpoint deletes all the screenings for a movie
        /// </summary>
        /// <param name="id">The id of the movie for which user wants to deletes all the screenings.</param>
        /// <returns>
        /// <example>
        /// DELETE: api/Screenings/RemoveScreeningsForMovie/10 -> No Content
        /// </example>
        /// Screenings are successfully deleted -> No Content
        /// No screening for the provided movie id exists -> Not Found
        /// </returns>

        [HttpDelete(template: "RemoveScreeningsForMovie/{id}")]
        public async Task<ActionResult> RemoveScreeningsForMovie(int id)
        {
            var screenings = await _context.Screenings.Where(s => s.MovieId == id).ToListAsync();
            if (screenings == null)
            {
                return NotFound();
            }
            foreach (var screening in screenings)
            {
                _context.Screenings.Remove(screening);

                await _context.SaveChangesAsync();
            }

            return NoContent();
        }

        /// <summary>
        /// This api endpoint deletes all the screenings taking place in a hall
        /// </summary>
        /// <param name="id">The id of the hall for which user wants to deletes all the screenings.</param>
        /// <returns>
        /// <example>
        /// DELETE: api/Screenings/RemoveScreeningsForHall/3 -> No Content
        /// </example>
        /// Screenings are successfully deleted -> No Content
        /// No screening for the provided hall id exists -> Not Found
        /// </returns>

        [HttpDelete(template: "RemoveScreeningsForHall/{id}")]

        public async Task<ActionResult> RemoveScreeningsForHall(int id)
        {
            var screenings = await _context.Screenings.Where(s => s.HallId == id).ToListAsync();

            if(screenings == null)
            {
                return NotFound();
            }

            foreach (var screening in screenings)
            {
                _context.Screenings.Remove(screening);

                await _context.SaveChangesAsync();
            }

            return NoContent();
        }

        private bool ScreeningExists(int id)
        {
            return _context.Screenings.Any(e => e.ScreeningId == id);
        }
    }
}
