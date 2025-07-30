using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using api.models;
using api.models.dtos;
using api.services;
using Microsoft.AspNetCore.Mvc;

namespace api.controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StationsController : ControllerBase
    {
        private readonly IStationService _stationService;

        public StationsController(IStationService stationService)
        {
            _stationService = stationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
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

        [HttpGet("by-order/{order:int}")]
        public async Task<IActionResult> GetByOrder(int order)
        {
            var station = await _stationService.GetByOrderAsync(order);
            if (station == null) return NotFound();
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

        [HttpPut("{id:guid}")]
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

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var success = await _stationService.DeleteStationAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }

        [HttpDelete("by-order/{order:int}")]
        public async Task<IActionResult> DeleteByOrder(int order)
        {
            var stationToDelete = await _stationService.GetByOrderAsync(order);
            if (stationToDelete == null)
            {
                return NotFound("Station with the specified order not found.");
            }

            var success = await _stationService.DeleteStationAsync(stationToDelete.StationId);
            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}