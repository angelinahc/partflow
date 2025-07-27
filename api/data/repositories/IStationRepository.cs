using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.models;

namespace api.data.repositories
{
    public interface IStationRepository
    {
        Task<Station?> GetByIdAsync(Guid id); // Search one specific station by its Id
        Task<IEnumerable<Station>> GetAllAsync(); // Get all stations that exists
        Task AddAsync(Station station); // Create a new station
    }
}