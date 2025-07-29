using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.models;

namespace api.data.repositories
{
    public class InMemoryPartRepository : IPartRepository
    {
        private static readonly List<Part> _parts = new();

        // Add a new part into the list
        public Task AddAsync(Part part)
        {
            _parts.Add(part);
            return Task.CompletedTask;
        }

        // Returns all the active parts
        public Task<IEnumerable<Part>> GetAllAsync()
        {
            var activeParts = _parts.Where(p => p.IsActive);

            return Task.FromResult(activeParts.AsEnumerable());
        }

        // Returns all the parts that has a specific name
        public Task<IEnumerable<Part>> GetByNameAsync(string partName)
        {
            var parts = _parts.Where(p => p.PartName.Equals(partName, StringComparison.OrdinalIgnoreCase));
            return Task.FromResult(parts);
        }

        // Return the part that has a specific number
        public Task<Part?> GetByNumberAsync(string partNumber)
        {
            var part = _parts.FirstOrDefault(p => p.PartNumber.Equals(partNumber, StringComparison.OrdinalIgnoreCase));
            return Task.FromResult(part);
        }

        // Edit a part
        public Task UpdateAsync(Part part)
        {
            var existingPart = _parts.FirstOrDefault(p => p.PartId == part.PartId);
            if (existingPart != null)
            {
                existingPart.CurrentStationId = part.CurrentStationId;
                existingPart.PartName = part.PartName;
                existingPart.PartNumber = part.PartNumber;
                existingPart.IsActive = part.IsActive;
                existingPart.IsCompleted = part.IsCompleted;
            }
            return Task.CompletedTask;
        }

        // Check if the part exists
        public Task<bool> PartNumberExistsAsync(string partNumber)
        {
            bool exists = _parts.Any(p => p.PartNumber.Equals(partNumber, StringComparison.OrdinalIgnoreCase));
            return Task.FromResult(exists);
        }
    }
}