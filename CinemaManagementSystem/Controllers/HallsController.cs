using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CinemaManagementSystem.Data;
using CinemaManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace CinemaManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HallsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public HallsController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// This API gives the List of all the halls in the database. 
        /// </summary>
        /// <example>
        /// GET: api/Halls/GetHalls -> [{"hallId":1,"name":"x3j5","capacity":172,"location":"6","screenings":null},{"hallId":2,"name":"t35j3","capacity":180,"location":"3","screenings":null},{"hallId":3,"name":"k4b7","capacity":150,"location":"5","screenings":null},{"hallId":4,"name":"s2e7","capacity":185,"location":"2","screenings":null}]
        /// </example>
        /// <returns>
        /// The List of Halls.
        /// </returns>
        [HttpGet(template:"GetHalls")]
        public async Task<ActionResult<IEnumerable<Hall>>> GetHalls()
        {
            return await _context.Halls.ToListAsync();
        }

        /// <summary>
        /// This api endpoint finds a hall when the id of the hall is provided.
        /// </summary>
        /// <param name="id">The id of numeric type of the hall that user wants to get</param>
        /// <example>
        /// GET: api/Halls/FindHall/4 -> 
        /// {"hallId":3,"name":"k4b7","capacity":150,"location":"5","screenings":null}
        /// </example>
        /// <returns>
        /// Returns the Hall.
        /// </returns>

        [HttpGet(template:"FindHall/{id}")]

        public async Task <ActionResult<Hall>> FindHall(int id)
        {
            return await _context.Halls.FindAsync(id);
        }


        /// <summary>
        /// This api endpoint receives the information of a hall and inserts it into the database.
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
        ///{"name":""h8f98,"Capacity":105,"Location":"14"}
        ///
        /// </example>
        /// <returns>
        /// Returns the hall that has just been added.
        /// </returns>
        [HttpPost(template:"AddHall")]

        public async Task <ActionResult<Hall>> AddHall(Hall hall)
        {
            _context.Halls.Add(hall);

            await _context.SaveChangesAsync();

            return CreatedAtAction("FindHall", new {id = hall.HallId} , hall);
        }

        /// <summary>
        /// This endpoint updates a hall in the database.
        /// </summary>
        /// <param name="id">The id of the hall that the user wants to update.</param>
        /// <param name="hall">The hall data that user wants to change.</param>
        /// <example>
        /// PUT: api/Hall/EditHall/3
        /// Headers: Content-Type: application/json
        /// Request Body:
        /// {
        ///     "HallId":3,
        ///     "Name":"s34t3,
        ///     "Capacity":90,
        ///     "Location":"12"
        /// }
        /// </example>
        /// <returns>
        /// Hall is successfully updated -> No Content
        /// Hall with the provided id does not exist -> Not Found
        /// The id of the hall does not match the id parameter -> Bad Request
        /// </returns>

        [HttpPut("{id}")]
        public async Task <IActionResult> EditHall(int id, Hall hall)
        {
            if(id != hall.HallId)
            {
                return BadRequest();
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
                    return BadRequest();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        /// <summary>
        /// This api endpoints deletes a hall from the database with the help of id.
        /// </summary>
        /// <param name="id">The id of the hall as an int that user wants to delete.</param>
        /// <example>
        /// 
        /// DELETE: api/Hall/DeleteHall/10 -> No Content
        /// 
        /// </example>
        /// <returns>
        /// Hall is successfully deleted -> No Content
        /// Hall with the provided id does not exist -> Not Found 
        /// </returns>


        [HttpDelete(template:"DeleteHall/{id}")]

        public async Task<ActionResult> DeleteHall(int id)
        {
            var hall = await _context.Halls.FindAsync(id);

            if(hall == null)
            {
                return NotFound();
            }

            _context.Halls.Remove(hall);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET - api/Halls/ListHallsForMovie/9

        /// <summary>
        /// This api endpoint returns the List of Halls that in which a movie is screened
        /// </summary>
        /// <param name="id">The id of the movie which user wants to know in which halls is being screened.</param>
        /// <example>
        /// GET: api/Halls/ListHallsForMovie/10 ->
        /// [{"hallId":1,"name":"x3j5","capacity":172,"location":"6","screenings":null}]
        /// </example>
        /// <returns>
        /// The List of the halls in the movie is screened of which id is given.
        /// </returns>

        [HttpGet(template:"ListHallsForMovie/{id}")]
        public async Task<ActionResult<IEnumerable<Hall>>> ListHallsForMovie(int id)
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
