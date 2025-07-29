using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.models;
using api.models.dtos;
using api.services;
using Microsoft.AspNetCore.Mvc;

namespace api.controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class StationsController : ControllerBase
    {
        private readonly IStationService _stationService;

        public StationsController(IStationService stationService)
        {
            _stationService = stationService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Station>>> GetAll()
        {
            var stations = await _stationService.GetAllStationAsync();
            return Ok(stations);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var station = await _stationService.GetByIdAsync(id);
            if (station == null) return NotFound();
            return Ok(station);
        }

        [HttpGet("by-order/{order}")]
        public async Task<IActionResult> GetByOrder(int order)
        {
            var station = await _stationService.GetByOrderAsync(order);
            if (station == null)
            {
                return NotFound();
            }
            return Ok(station);
        }

        [HttpGet("name/{stationName}")]
        public async Task<IActionResult> GetByName(string stationName)
        {
            var station = await _stationService.GetByNameAsync(stationName);
            if (station == null) return NotFound();
            return Ok(station);
        }

        [HttpPost]
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

        [HttpPut("{id}")]
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

        [HttpDelete("by{id}")]
        public async Task<IActionResult> DeleteById(Guid id)
        {
            var success = await _stationService.DeleteByIdAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }

        [HttpDelete("{order}")]
        public async Task<IActionResult> DeleteByOrder(int order)
        {
            var success = await _stationService.DeleteByOrderAsync(order);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}