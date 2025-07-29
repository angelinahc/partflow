using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.models;
using api.models.dtos;

namespace api.services
{
    public interface IPartService
    {
        // Create a new part
        Task<Part> CreatePartAsync(string partNumber, string partName);

        // Move the part to the next station
        Task<bool> MovePartAsync(string partNumber, string responsible);

        // Get the history of a part by its number
        Task<IEnumerable<FlowHistoryDto>> GetPartHistoryAsync(string partNumber);

        // Removes a part by its number
        Task<bool> DeletePartAsync(string partNumber);

        // Simple queries
        Task<PartDto?> GetPartByNumberAsync(string partNumber);
        Task<IEnumerable<PartDto>> GetAllPartsAsync();
    }
}