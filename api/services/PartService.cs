using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.data.repositories;
using api.models;
using Microsoft.AspNetCore.SignalR;

namespace api.services
{
    public class PartService : IPartService
    {
        private readonly IPartRepository _partRepository;
        private readonly IStationRepository _stationRepository;
        private readonly IFlowHistoryRepository _historyRepository;

        // O construtor recebe todas as "ferramentas" que precisa para trabalhar
        public PartService(
            IPartRepository partRepository,
            IStationRepository stationRepository,
            IFlowHistoryRepository historyRepository)
        {
            _partRepository = partRepository;
            _stationRepository = stationRepository;
            _historyRepository = historyRepository;
        }

        // Create a new part
        public async Task<Part> CreatePartAsync(string partNumber, string partName)
        {
            // Verify if part already exists
            bool partExists = await _partRepository.PartNumberExistsAsync(partNumber);
            if (partExists)
            {
                throw new InvalidOperationException($"Uma peça com o número '{partNumber}' já existe.");
            }

            var newPart = new Part
            {
                PartNumber = partNumber,
                PartName = partName
            };

            // Add part into the repository
            await _partRepository.AddAsync(newPart);

            return newPart;
        }

        // Get all parts
        public Task<IEnumerable<Part>> GetAllPartsAsync()
        {
            return _partRepository.GetAllAsync();
        }

        // Get a part by its number
        public Task<Part?> GetPartByNumberAsync(string partNumber)
        {
            return _partRepository.GetByNumberAsync(partNumber);
        }

        // Get the history of a part by its number
        public async Task<IEnumerable<FlowHistory>> GetPartHistoryAsync(string partNumber)
        {
            var part = await _partRepository.GetByNumberAsync(partNumber);

            if (part == null)
            {
               return Enumerable.Empty<FlowHistory>();
            }

            return await _historyRepository.GetByPartIdAsync(part.PartId);
        }

        // Implementing the logic of part movement
        public async Task<bool> MovePartAsync(string partNumber, string responsible)
        {
            var part = await _partRepository.GetByNumberAsync(partNumber); // Find the part
            if (part == null) return false;

            if (part.Status == Status.Completed) return false; // Part alredy finished

            var stations = (await _stationRepository.GetAllAsync())
                                .OrderBy(s => s.Order).ToList();

            // Find the actual station
            var currentStation = stations.FirstOrDefault(s => s.Order == (int)part.Status + 1);
            if (currentStation == null) return false; // Station not found

            // See if the new station is valid
            var newStation = stations.FirstOrDefault(s => s.Order == currentStation.Order + 1);
            if (newStation != null)
            {
                part.Status = (Status)Enum.Parse(typeof(Status), newStation.StationName, true);

                var flow = new FlowHistory
                {
                    PartId = part.PartId,
                    FromStationId = currentStation.StationId,
                    ToStationId = newStation.StationId,
                    Collaborator = responsible
                };
                await _historyRepository.AddAsync(flow); // Register at the history
            }
            else
            {
                part.Status = Status.Completed;
            }

            await _partRepository.UpdateAsync(part); // Save the new station
            return true;
        }

        // Delete a part by its number
        public async Task<bool> DeletePartAsync(string partNumber)
        {
            var part = await _partRepository.GetByNumberAsync(partNumber);
            if (part == null)
            {
                return false;
            }

            part.IsActive = false;
            await _partRepository.UpdateAsync(part);
            return true;
        }
    }
}