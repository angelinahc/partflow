using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.models;

namespace api.data.repositories
{
    public interface IPartRepository
    {
        Task<Part?> GetByNumberAsync(string partNumber); // Search a part by its number
        Task<IEnumerable<Part>> GetByNameAsync(string partName); // Search all parts that has a specific name
        Task<IEnumerable<Part>> GetAllAsync(); // Return all parts that exists
        Task AddAsync(Part part); // Add a new part
        Task UpdateAsync(Part part); // Edit an existing part
        Task<bool> PartNumberExistsAsync(string partNumber); // Verifies the existence of a part
    }
}