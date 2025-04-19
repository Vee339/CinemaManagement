using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CinemaManagementSystem.Data;
using CinemaManagementSystem.Models;
using CinemaManagementSystem.Interfaces;
using CinemaManagementSystem.Services;
using Microsoft.EntityFrameworkCore;
using System.Net.WebSockets;

namespace CinemaManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HallsController : ControllerBase
    {
        private readonly IHallsService _hallsService;

        public HallsController(IHallsService HallsService)
        {
            _hallsService = HallsService;
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
            IEnumerable<Hall> Halls = await _hallsService.GetHalls();

            return Ok(Halls);
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

        [HttpGet(template: "FindHall/{id}")]

        public async Task<ActionResult<Hall>> FindHall(int id)
        {
            return await _hallsService.FindHall(id);
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
        [HttpPost(template: "AddHall")]

        public async Task<ActionResult<Hall>> AddHall(Hall hall)
        {
            

            await _hallsService.AddHall(hall);

            return NoContent();
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
        public async Task<IActionResult> EditHall(int id, Hall hall)
        {

            var result = await _hallsService.EditHall(id, hall);
            
            if(result == "Bad Request")
            {
                return BadRequest();
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


        [HttpDelete(template: "DeleteHall/{id}")]

        public async Task<ActionResult> DeleteHall(int id)
        {
            var result = await _hallsService.DeleteHall(id);

            if (result == "Not Found")
            {
                return NotFound();
            }


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

        [HttpGet(template: "ListHallsForMovie/{id}")]
        public async Task<ActionResult<IEnumerable<Hall>>> ListHallsForMovie(int id)
        {
            IEnumerable<Hall> Halls = await _hallsService.ListHallsForMovie(id);

            return Ok(Halls);
        }



    }
}
