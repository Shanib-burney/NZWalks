using NZWalksAPI.Models.Domain;
using NZWalksAPI.Models.DTO;

namespace NZWalksAPI.Repositories
{
    public interface IWalkRepository
    {
        Task<List<Walk>> GetAllAsync(string? filterOn = null, string? filterQuery = null, string? sortBy = null, bool isAscending = true, int pageNumber = 1, int pageSize = 100);

        Task<Walk?> GetByIdAsync(Guid id);

        Task<Walk> CreateAsync(Walk walk);

        Task<Walk?> UpdateAsync(Guid id, WalkRequestDTO walkUpdate);

        Task<Walk?> DeleteAsync(Guid id);
        
    }
}


