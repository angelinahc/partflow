using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.models;
using api.models.dtos;

namespace api.services
{
    public interface IStationService
    {
        Task<IEnumerable<Station>> GetAllStationAsync();
        Task<Station?> GetByNameAsync(string name);
        Task<Station?> GetByIdAsync(Guid id);
        Task<Station?> GetByOrderAsync(int order);
        Task<Station> CreateStationAsync(CreateStationDto stationDto);
        Task<Station?> UpdateStationAsync(Guid id, UpdateStationDto stationDto);
        Task<bool> DeleteByIdAsync(Guid id);
        Task<bool> DeleteByOrderAsync(int order);
    }
}   