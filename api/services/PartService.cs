using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.data.repositories;
using api.models;
using api.models.dtos;
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
            var partWithSameName = (await _partRepository.GetByNameAsync(partName))
                                    .FirstOrDefault(p => p.IsActive);

            var partWithSameNumber = await _partRepository.GetByNumberAsync(partNumber);

            if (partWithSameName != null && partWithSameName.PartId != partWithSameNumber?.PartId)
            {
                throw new InvalidOperationException($"A part with the name '{partName}' already exists.");
            }
            
            if (partWithSameNumber != null)
            {
                if (partWithSameNumber.IsActive)
                {
                    throw new InvalidOperationException($"A part with number '{partNumber}' already exists and is active.");
                }
                else
                {
                    partWithSameNumber.IsActive = true;
                    partWithSameNumber.IsCompleted = false;
                    partWithSameNumber.CurrentStationId = null;
                    partWithSameNumber.PartName = partName;
                    
                    await _partRepository.UpdateAsync(partWithSameNumber);
                    return partWithSameNumber;
                }
            }
            else
            {
                var newPart = new Part
                {
                    PartNumber = partNumber,
                    PartName = partName
                };

                await _partRepository.AddAsync(newPart);
                return newPart;
            }
        }

        // Get all parts
        public async Task<IEnumerable<PartDto>> GetAllPartsAsync()
        {
            var parts = await _partRepository.GetAllAsync();
            var allStations = await _stationRepository.GetAllAsync(); // Pega as estações uma vez

            // Usa .Select do LINQ para aplicar a mesma lógica de mapeamento para cada item da lista
            return parts.Select(part => new PartDto
            {
                PartId = part.PartId,
                PartNumber = part.PartNumber,
                PartName = part.PartName,
                IsActive = part.IsActive,
                Status = part.IsCompleted
                    ? "Completed"
                    : part.CurrentStationId.HasValue
                        ? allStations.FirstOrDefault(s => s.StationId == part.CurrentStationId.Value)?.StationName ?? "Invalid station"
                        : "Out of Process"
            });
        }

        // Get a part by its number
        public async Task<PartDto?> GetPartByNumberAsync(string partNumber)
        {
            var part = await _partRepository.GetByNumberAsync(partNumber);
            if (part == null) return null;

            // Para fazer o mapeamento, precisamos da lista de estações
            var allStations = await _stationRepository.GetAllAsync();

            var partDto = new PartDto
            {
                PartId = part.PartId,
                PartNumber = part.PartNumber,
                PartName = part.PartName,
                IsActive = part.IsActive,
                Status = part.IsCompleted
                    ? "Completed"
                    : part.CurrentStationId.HasValue
                        ? allStations.FirstOrDefault(s => s.StationId == part.CurrentStationId.Value)?.StationName ?? "Invalid station"
                        : "Out of process"
            };

            return partDto;
        }

        // Get the history of a part by its number
        public async Task<IEnumerable<FlowHistoryDto>> GetPartHistoryAsync(string partNumber)
        {
            var part = await _partRepository.GetByNumberAsync(partNumber);
            if (part == null)
            {
                return Enumerable.Empty<FlowHistoryDto>();
            }

            var historyRecords = await _historyRepository.GetByPartIdAsync(part.PartId);
            
            var allStations = (await _stationRepository.GetAllAsync()).ToList();

            var historyDtos = historyRecords.Select(record => {
                var fromStation = allStations.FirstOrDefault(s => s.StationId == record.FromStationId);
                var toStation = allStations.FirstOrDefault(s => s.StationId == record.ToStationId);

                return new FlowHistoryDto
                {
                    FromStationName = fromStation?.StationName ?? "Estação Desconhecida",
                    ToStationName = record.ToStationId == Guid.Empty
                                    ? "Completed" 
                                    : toStation?.StationName ?? "Estação Desconhecida",

                    MovementDate = record.Date,
                    Responsible = record.Collaborator
                };
            }).ToList();

            return historyDtos;
        }

        // Logic of part movement
        public async Task<bool> MovePartAsync(string partNumber, string responsible)
        {
            var part = await _partRepository.GetByNumberAsync(partNumber);
            if (part == null || part.IsCompleted) return false; // Não pode mover peça inexistente ou finalizada

            var stations = (await _stationRepository.GetAllAsync()).OrderBy(s => s.Order).ToList();
            if (!stations.Any()) return false; // Não há estações cadastradas

            Station? currentStation = null;
            if (part.CurrentStationId.HasValue)
            {
                currentStation = stations.FirstOrDefault(s => s.StationId == part.CurrentStationId.Value);
            }

            Station? nextStation;
            if (currentStation == null)
            {
                nextStation = stations.First();
            }
            else
            {
                nextStation = stations.FirstOrDefault(s => s.Order > currentStation.Order);
            }
            
            var movement = new FlowHistory
            {
                PartId = part.PartId,
                FromStationId = currentStation?.StationId ?? Guid.Empty,
                ToStationId = nextStation?.StationId ?? Guid.Empty,
                Collaborator = responsible
            };
            await _historyRepository.AddAsync(movement);

            if (nextStation != null)
            {
                part.CurrentStationId = nextStation.StationId;
            }
            else
            {
                part.CurrentStationId = null;
                part.IsCompleted = true;
            }

            await _partRepository.UpdateAsync(part);
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