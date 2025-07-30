using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.data.repositories;
using api.models;
using api.models.dtos;
using Microsoft.AspNetCore.Hosting.StaticWebAssets;
using Microsoft.AspNetCore.Http.HttpResults;

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

        public Task<Station?> GetByNameAsync(string stationName)
        {
            return _stationRepository.GetByNameAsync(stationName);
        }

        public Task<Station?> GetByIdAsync(Guid id)
        {
            return _stationRepository.GetByIdAsync(id);
        }

        public Task<Station?> GetByOrderAsync(int order)
        {
            return _stationRepository.GetByOrderAsync(order);
        }

        // Creating a new station and reordering the list
        public async Task<Station> CreateStationAsync(CreateStationDto stationDto)
        {
            bool nameExists = await _stationRepository.StationNameExistsAsync(stationDto.StationName);
            if (nameExists)
            {
                throw new InvalidOperationException($"The station '{stationDto.StationName}' already exists.");
            }

            var allStations = (await _stationRepository.GetAllAsync()).ToList();

            // Find all stations whose order is greater than or equal to the desired order
            var stationsToShift = allStations.Where(s => s.Order >= stationDto.Order);

            // "Push" each one a position forward
            foreach (var station in stationsToShift.OrderByDescending(s => s.Order))
            {
                station.Order++;
                await _stationRepository.UpdateAsync(station);
            }

            var newStation = new Station
            {
                StationName = stationDto.StationName,
                Description = stationDto.Description,
                Location = stationDto.Location,
                Order = stationDto.Order
            };

            await _stationRepository.AddAsync(newStation);

            return newStation;
        }

        // Update a station
        public async Task<Station?> UpdateStationAsync(Guid id, UpdateStationDto stationDto)
        {
            // Validação de nome duplicado (importante!)
            var existingStationWithName = await _stationRepository.GetByNameAsync(stationDto.StationName);
            if (existingStationWithName != null && existingStationWithName.StationId != id)
            {
                throw new InvalidOperationException($"Uma estação com o nome '{stationDto.StationName}' já existe.");
            }

            var stationToUpdate = await _stationRepository.GetByIdAsync(id);
            if (stationToUpdate == null)
            {
                return null;
            }

            stationToUpdate.StationName = stationDto.StationName;
            stationToUpdate.Description = stationDto.Description;
            stationToUpdate.Location = stationDto.Location;

            await _stationRepository.UpdateAsync(stationToUpdate);
            return stationToUpdate;
        }

        public async Task<bool> DeleteStationAsync(Guid id)
        {
            var stationToDelete = await _stationRepository.GetByIdAsync(id);
            if (stationToDelete == null || !stationToDelete.IsActive)
            {
                return false;
            }

            int deletedOrder = stationToDelete.Order;

            stationToDelete.IsActive = false;
            await _stationRepository.UpdateAsync(stationToDelete);

            var stationsToShift = (await _stationRepository.GetAllAsync())
                                        .Where(s => s.Order > deletedOrder)
                                        .ToList();

            foreach (var station in stationsToShift)
            {
                station.Order--;
                await _stationRepository.UpdateAsync(station);
            }

            return true;
        }

        public async Task<bool> DeleteByOrderAsync(int order)
        {
            var station = await _stationRepository.GetByOrderAsync(order);
            if (station == null)
            {
                return false;
            }

            station.IsActive = false;
            await _stationRepository.UpdateAsync(station);
            return true;
        }
    }
}