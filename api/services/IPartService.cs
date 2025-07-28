using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.models;

namespace api.services
{
    public interface IPartService
    {
        // Create a new part
        Task<Part> CreatePartAsync(string partNumber, string partName);

        // Move the part to the next station
        Task<bool> MovePartAsync(string partNumber, string responsible);

        // Get the history of a part by its number
        Task<IEnumerable<FlowHistory>> GetPartHistoryAsync(string partNumber);

        // Removes a part by its number
        Task<bool> DeletePartAsync(string partNumber);

        // Simple queries
        Task<Part?> GetPartByNumberAsync(string partNumber);
        Task<IEnumerable<Part>> GetAllPartsAsync();
    }
}