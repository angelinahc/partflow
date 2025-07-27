using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.models;

namespace api.data.repositories
{
    public class InMemoryStationRepository : IStationRepository
    {
        private static readonly List<Station> _stations = new(); // Create a static list of movements, simulating a database

        // Adding the stations correctly if the list is blank
        public InMemoryStationRepository()
        {
            if (_stations.Count == 0)
            {
                _stations.Add(new Station { StationName = "Receiving", Order = 1 });
                _stations.Add(new Station { StationName = "Assembly", Order = 2 });
                _stations.Add(new Station { StationName = "FinalInspection", Order = 3 });
            }
        }

        // Adding a new station to the list
        public Task AddAsync(Station station)
        {
            _stations.Add(station);
            return Task.CompletedTask;
        }
        // Return all stations
        public Task<IEnumerable<Station>> GetAllAsync()
        {
            return Task.FromResult(_stations.AsEnumerable());
        }

        // Returns the first value that satisfies a condition, or returns a default value, null
        public Task<Station?> GetByIdAsync(Guid id)
        {
            var station = _stations.FirstOrDefault(s => s.StationId == id);
            return Task.FromResult(station);
        }
    }
}