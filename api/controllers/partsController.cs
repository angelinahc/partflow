using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.controllers.dtos;
using api.dtos;
using api.models;
using api.models.dtos;
using api.services;
using Microsoft.AspNetCore.Mvc;

namespace api.controller
{
    /// <summary>
    /// Manages part-related operations, including creation, retrieval, and movement.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class PartsController : ControllerBase
    {
        private readonly IPartService _partService;

        public PartsController(IPartService partService)
        {
            _partService = partService;
        }

        /// <summary>
        /// Creates a new part or reactivates an existing inactive one.
        /// </summary>
        /// <remarks>
        /// If a part with the same number exists but is inactive, it will be reactivated.
        /// Throws a conflict error if the part number is already in use by an active part.
        /// </remarks>
        /// <param name="partDto">The data for the new part.</param>
        /// <returns>The created or reactivated part.</returns>
        /// <response code="201">Returns the newly created part.</response>
        /// <response code="409">If an active part with the same number already exists.</response>
        [HttpPost]
        [ProducesResponseType(typeof(Part), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CreatePart([FromBody] CreatePartDto partDto)
        {
            try
            {
                var newPart = await _partService.CreatePartAsync(partDto.PartNumber, partDto.PartName);
                return CreatedAtAction(nameof(GetByNumber), new { partNumber = newPart.PartNumber }, newPart);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Retrieves a specific part by its unique number.
        /// </summary>
        /// <param name="partNumber">The unique number of the part.</param>
        /// <returns>The requested part details.</returns>
        /// <response code="200">Returns the part details.</response>
        /// <response code="404">If the part is not found.</response>
        [HttpGet("{partNumber}")]
        [ProducesResponseType(typeof(PartDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PartDto>> GetByNumber(string partNumber)
        {
            var part = await _partService.GetPartByNumberAsync(partNumber);
            if (part == null)
            {
                return NotFound();
            }
            return Ok(part);
        }

        /// <summary>
        /// Retrieves a list of all active parts.
        /// </summary>
        /// <returns>A list of parts.</returns>
        /// <response code="200">Returns the list of active parts.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<PartDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<PartDto>>> GetAll()
        {
            var parts = await _partService.GetAllPartsAsync();
            return Ok(parts);
        }

        /// <summary>
        /// Retrieves the movement history for a specific part.
        /// </summary>
        /// <param name="partNumber">The unique number of the part.</param>
        /// <returns>A list of historical movements for the part.</returns>
        /// <response code="200">Returns the movement history.</response>
        [HttpGet("{partNumber}/history")]
        [ProducesResponseType(typeof(IEnumerable<FlowHistoryDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<FlowHistoryDto>>> GetHistory(string partNumber)
        {
            var history = await _partService.GetPartHistoryAsync(partNumber);
            return Ok(history);
        }

        /// <summary>
        /// Moves a part to the next station in the workflow.
        /// </summary>
        /// <param name="partNumber">The unique number of the part to move.</param>
        /// <param name="moveDto">An object containing the name of the responsible collaborator.</param>
        /// <returns>A confirmation message.</returns>
        /// <response code="200">If the part was moved successfully.</response>
        /// <response code="400">If the part cannot be moved (e.g., it is already completed or does not exist).</response>
        [HttpPost("{partNumber}/move")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Move(string partNumber, [FromBody] MovePartDto moveDto)
        {
            var success = await _partService.MovePartAsync(partNumber, moveDto.Responsible);
            if (!success)
            {
                return BadRequest(new { message = "The part could not be moved. Check the number or current status of the part." });
            }
            return Ok(new { message = $"Part {partNumber} has been moved successfully." });
        }

        /// <summary>
        /// Deactivates a part (soft delete).
        /// </summary>
        /// <param name="partNumber">The unique number of the part to delete.</param>
        /// <returns>No content if successful.</returns>
        /// <response code="204">If the part was successfully deactivated.</response>
        /// <response code="404">If the part to delete was not found.</response>
        [HttpDelete("{partNumber}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(string partNumber)
        {
            var success = await _partService.DeletePartAsync(partNumber);
            if (!success)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}