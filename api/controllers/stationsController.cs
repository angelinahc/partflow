using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using api.models;
using api.models.dtos;
using api.services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace api.controllers
{
    /// <summary>
    /// Manages all operations related to process stations.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class StationsController : ControllerBase
    {
        private readonly IStationService _stationService;

        public StationsController(IStationService stationService)
        {
            _stationService = stationService;
        }

        /// <summary>
        /// Retrieves a list of all active stations, ordered by their process order.
        /// </summary>
        /// <returns>A list of active stations.</returns>
        /// <response code="200">Returns the list of stations.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Station>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var stations = await _stationService.GetAllStationAsync();
            return Ok(stations);
        }

        /// <summary>
        /// Retrieves a specific station by its unique ID (GUID).
        /// </summary>
        /// <param name="id">The unique ID of the station.</param>
        /// <returns>The requested station.</returns>
        /// <response code="200">Returns the station details.</response>
        /// <response code="404">If the station is not found.</response>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(Station), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var station = await _stationService.GetByIdAsync(id);
            if (station == null) return NotFound();
            return Ok(station);
        }

        /// <summary>
        /// Retrieves a specific station by its process order number.
        /// </summary>
        /// <param name="order">The order number of the station in the workflow.</param>
        /// <returns>The requested station.</returns>
        /// <response code="200">Returns the station details.</response>
        /// <response code="404">If no station is found at that order number.</response>
        [HttpGet("by-order/{order:int}")]
        [ProducesResponseType(typeof(Station), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByOrder(int order)
        {
            var station = await _stationService.GetByOrderAsync(order);
            if (station == null) return NotFound();
            return Ok(station);
        }
        
        /// <summary>
        /// Retrieves a specific station by its name (case-insensitive).
        /// </summary>
        /// <param name="stationName">The name of the station.</param>
        /// <returns>The requested station.</returns>
        /// <response code="200">Returns the station details.</response>
        /// <response code="404">If the station is not found.</response>
        [HttpGet("name/{stationName}")]
        [ProducesResponseType(typeof(Station), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByName(string stationName)
        {
            var station = await _stationService.GetByNameAsync(stationName);
            if (station == null) return NotFound();
            return Ok(station);
        }

        /// <summary>
        /// Creates a new station and reorders subsequent stations if necessary.
        /// </summary>
        /// <param name="stationDto">The data for the new station.</param>
        /// <returns>The newly created station.</returns>
        /// <response code="201">Returns the newly created station.</response>
        /// <response code="409">If a station with the same name already exists.</response>
        [HttpPost]
        [ProducesResponseType(typeof(Station), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Create([FromBody] CreateStationDto stationDto)
        {
            try
            {
                var newStation = await _stationService.CreateStationAsync(stationDto);
                return CreatedAtAction(nameof(GetById), new { id = newStation.StationId }, newStation);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Updates an existing station's properties.
        /// </summary>
        /// <param name="id">The unique ID of the station to update.</param>
        /// <param name="stationDto">The updated data for the station.</param>
        /// <returns>The updated station object.</returns>
        /// <response code="200">Returns the updated station.</response>
        /// <response code="404">If the station to update is not found.</response>
        /// <response code="409">If the new station name conflicts with an existing one.</response>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(Station), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateStationDto stationDto)
        {
            try
            {
                var updatedStation = await _stationService.UpdateStationAsync(id, stationDto);
                if (updatedStation == null) return NotFound();
                return Ok(updatedStation);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Deactivates a station by its unique ID (soft delete) and reorders subsequent stations.
        /// </summary>
        /// <param name="id">The unique ID of the station to delete.</param>
        /// <returns>No content if successful.</returns>
        /// <response code="204">If the station was successfully deactivated.</response>
        /// <response code="404">If the station to delete is not found.</response>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var success = await _stationService.DeleteStationAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}