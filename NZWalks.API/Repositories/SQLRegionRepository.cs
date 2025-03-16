using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class SQLRegionRepository : IRegionRepository
    {
        private readonly NZWalksDbContext _dbContext;

        public SQLRegionRepository(NZWalksDbContext _dbContext)
        {
            this._dbContext = _dbContext;
        }

        public async Task<Region> CreateAsync(Region region)
        {
            await _dbContext.Regions.AddAsync(region);
            await _dbContext.SaveChangesAsync();
            return region;
        }

        public async Task<Region?> DeleteAsync(Guid id)
        {
            var regionDomainModel = await _dbContext.Regions.FirstOrDefaultAsync(u => u.Id == id);
            if(regionDomainModel == null)
            {
                return null;
            }

            _dbContext.Regions.Remove(regionDomainModel);
            await _dbContext.SaveChangesAsync();

            return regionDomainModel;
        }

        public async Task<List<Region>> GetAllAsync()
        {
            return await _dbContext.Regions.ToListAsync();
        }

        public async Task<Region?> GetByIdAsync(Guid id)
        {
            return await _dbContext.Regions.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<Region?> UpdateAsync(Guid id, Region region)
        {
            var existingRegion = await _dbContext.Regions.FirstOrDefaultAsync(u => u.Id == id);

            if(existingRegion == null)
            {
                return null;
            }

            // Map DTO to Domain model 
            existingRegion.Code = region.Code;
            existingRegion.Name = region.Name;
            existingRegion.RegionImageUrl = region.RegionImageUrl;

            await _dbContext.SaveChangesAsync();

            return existingRegion;
        }
    }
}
