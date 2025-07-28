using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.data.repositories;
using api.models;

namespace api.services
{
    public class StationService : IStationService
    {
        private readonly IStationRepository _stationRepository;

        public StationService(IStationRepository stationRepository)
        {
            _stationRepository = stationRepository;
        }

        public Task<IEnumerable<Station>> GetAllStationAsync()
        {
            return _stationRepository.GetAllAsync();
        }
    }
}