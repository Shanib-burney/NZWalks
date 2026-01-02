using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NZWalksAPI.Data;
using NZWalksAPI.Models.Domain;
using NZWalksAPI.Models.DTO;

namespace NZWalksAPI.Repositories
{
    public class SQLRegionRepository : IRegionRepository

    {
        private readonly NZWalksDbContext dbContext;
        private readonly IMapper mapper;

        public SQLRegionRepository(NZWalksDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public async Task<Region> CreateAsync(Region region)
        {
            await dbContext.Regions.AddAsync(region);
            await dbContext.SaveChangesAsync();
            return region;
        }

     

        public async Task<List<Region>> GetAllAsync()
        {
           return await dbContext.Regions.ToListAsync();
        }

        public async Task<Region?> GetByIdAsync(Guid id)
        {
            return await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
         }

        public async Task<Region?> UpdateAsync(Guid id, RegionRequestDTO regionUpdate)
        { 
            var existingRegion = await this.GetByIdAsync(id);
            if (existingRegion == null) return null;

            // Source , Destination
            mapper.Map(regionUpdate, existingRegion);

            await dbContext.SaveChangesAsync();

            return existingRegion;

        }

        public async Task<Region?> DeleteAsync(Guid id)
        {
            var region = await this.GetByIdAsync(id);

            if (region == null) return null;

            dbContext.Regions.Remove(region);
            await dbContext.SaveChangesAsync();

            return region;
        }

    }
}
