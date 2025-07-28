using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.controllers.dtos;
using api.dtos;
using api.models;
using api.services;
using Microsoft.AspNetCore.Mvc;

namespace api.controller
{
    [ApiController]
    [Route("api/[Controller]")]
    public class PartsController : ControllerBase
    {
        private readonly IPartService _partService;

        public PartsController(IPartService partService)
        {
            _partService = partService;
        }

        // Creates a new part
        [HttpPost]
        public async Task<IActionResult> CreatePart([FromBody] CreatePartDto partDto)
        {
            try
            {
                var newPart = await _partService.CreatePartAsync(partDto.PartNumber, partDto.PartName);
                return CreatedAtAction(nameof(GetByNumber), new { partNumber = newPart.PartNumber }, newPart);
            }
            catch (InvalidCastException error)
            {
                return Conflict(new { message = error.Message });
            }
        }

        // Get a part by its number
        [HttpGet("{partNumber}")]
        public async Task<IActionResult> GetByNumber(string partNumber)
        {
            var part = await _partService.GetPartByNumberAsync(partNumber);

            if (part == null)
            {
                return NotFound();
            }
            return Ok(part);
        }

        // Get all the parts
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var part = await _partService.GetAllPartsAsync();
            return Ok(part);
        }

        // Get history
        [HttpGet("{parNumber}/history")]
        public async Task<IActionResult> GetHistory(string partNumber)
        {
            var history = await _partService.GetPartHistoryAsync(partNumber);
            return Ok(history);
        }

        // Move a part to the new station
        [HttpPost("{partNumber}/move")]
        public async Task<IActionResult> Move(string partNumber, [FromBody] MovePartDto moveDto)
        {
            var success = await _partService.MovePartAsync(partNumber, moveDto.Reponsible);

            if (!success)
            {
                return BadRequest(new { message = "The part could not be moved. Check the number or current status of the part." });
            }

            return Ok(new { message = $"Part {partNumber} has been moved successfully" });
        }

        // Delete a part
        [HttpDelete("{partNumber}")]
        public async Task<IActionResult> Delete(string partNumber)
        {
            var success = await _partService.DeletePartAsync(partNumber);
            if (!success)
            {
                return NotFound();
            }
            return Ok(new { message = $"Part {partNumber} has been removed successfully" });
        }
    }
}