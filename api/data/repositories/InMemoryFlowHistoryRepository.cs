using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.models;

namespace api.data.repositories
{
    public class InMemoryFlowHistoryRepository : IFlowHistoryRepository
    {
        private static readonly List<FlowHistory> _histories = new();

        // Add a new movement
        public Task AddAsync(FlowHistory flow)
        {
            _histories.Add(flow);
            return Task.CompletedTask;
        }

        // Search a part history by its Id
        public Task<IEnumerable<FlowHistory>> GetByPartIdAsync(Guid id)
        {
            var partHistory = _histories
                                .Where(h => h.PartId == id)
                                .OrderBy(h => h.Date);

            return Task.FromResult(partHistory.AsEnumerable());
        }
    }
}