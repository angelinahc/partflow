using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.models;

namespace api.data.repositories
{
    public interface IFlowHistoryRepository
    {
        Task AddAsync(FlowHistory historyRecord); // Add a new record to the history
        Task<IEnumerable<FlowHistory>> GetByPartIdAsync(Guid partId); // Bring all the FlowHistory information by the PartId
    }
}