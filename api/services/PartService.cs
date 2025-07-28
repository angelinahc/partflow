using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.data.repositories;
using api.models;

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

        public Task<IEnumerable<Part>> GetAllPartsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Part?> GetPartByNumberAsync(string partNumber)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<FlowHistory>> GetPartHistoryAsync(string partNumber)
        {
            throw new NotImplementedException();
        }

        // Implementing the logic of part movement
        public async Task<bool> MovePartAsync(string partNumber, string responsible)
        {
            var part = await _partRepository.GetByNumberAsync(partNumber); // Find the part
            if (part == null) return false;

            if (part.Status ==Status.Completed) return false; // Part alredy finished

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
    }
}