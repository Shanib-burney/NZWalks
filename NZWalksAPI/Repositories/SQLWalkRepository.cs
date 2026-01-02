using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NZWalksAPI.Data;
using NZWalksAPI.Models.Domain;
using NZWalksAPI.Models.DTO;

namespace NZWalksAPI.Repositories
{
    public class SQLWalkRepository : IWalkRepository

    {
        private readonly NZWalksDbContext dbContext;
        private readonly IMapper mapper;

        public SQLWalkRepository(NZWalksDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }


        public async Task<Walk> CreateAsync(Walk walk)
        {
            await dbContext.Walks.AddAsync(walk);
            await dbContext.SaveChangesAsync();

            return walk;

        }

        public async Task<Walk?> DeleteAsync(Guid id)
        {
            var walk = await this.GetByIdAsync(id);
            if (walk == null)
            {
                return null;
            }
            dbContext.Walks.Remove(walk);
            await dbContext.SaveChangesAsync();
            return walk;


        }

        public async Task<List<Walk>> GetAllAsync(string? filterOn = null, string? filterQuery = null, string? sortBy = null, bool isAscending = true, int pageNumber = 1, int pageSize = 1000)
        {
          var walks =  dbContext.Walks.Include("Difficulty").Include("Region").AsQueryable();

         // Filtering
          if(string.IsNullOrWhiteSpace(filterOn) == false && string.IsNullOrWhiteSpace(filterQuery) == false)
            {
                if (filterOn.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    walks = walks.Where(x => x.Name.Contains(filterQuery));
                
                }
            }

          // Sorting

          if(string.IsNullOrWhiteSpace(sortBy) == false)
            {
                if(sortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    walks = isAscending ? walks.OrderBy(x => x.Name) : walks.OrderByDescending(x => x.Name);
                }else if (sortBy.Equals("Length", StringComparison.OrdinalIgnoreCase))
                {
                    walks = isAscending ? walks.OrderBy(x => x.LengthInKm) : walks.OrderByDescending(x => x.LengthInKm);

                }
            }

          // Pagination
          var skipResults = (pageNumber - 1) * pageSize;


            return await walks.Skip(skipResults).Take(pageSize).ToListAsync();
        }

        public async Task<Walk?> GetByIdAsync(Guid id)
        {
            return await dbContext.Walks
                .Include("Difficulty")
                .Include("Region")
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Walk?> UpdateAsync(Guid id, WalkRequestDTO walkUpdate)
        {
            {
                var existingWalk = await this.GetByIdAsync(id);
                if (existingWalk == null) return null;

                // Source , Destination
                mapper.Map(walkUpdate, existingWalk);

                await dbContext.SaveChangesAsync();

                return existingWalk;
            }
        }
    }
}
