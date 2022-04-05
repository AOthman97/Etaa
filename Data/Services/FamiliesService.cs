using Etaa.Models;
using Microsoft.EntityFrameworkCore;

namespace Etaa.Data.Services
{
    public class FamiliesService : IFamiliesService
    {
        private readonly ApplicationDbContext _dbContext;

        public FamiliesService(ApplicationDbContext applicationDbContext)
        {
            _dbContext = applicationDbContext;
        }

        public async Task AddFamilyAsync(Family family)
        {
            var Result = await _dbContext.Families.AddAsync(family);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Family>> GetAllAsync()
        {
            var Result = await _dbContext.Families.OrderBy(Family => Family.NameAr).ToListAsync();
            return Result;
        }

        public async Task<Family> GetFamilyAsync(int FamilyId)
        {
            var Result = await _dbContext.Families.FirstOrDefaultAsync(Family => Family.FamilyId == FamilyId);
            return Result;
        }

        public async Task<Family> UpdateFamilyAsync(int FamilyId, Family family)
        {
            _dbContext.Update(family);
            await _dbContext.SaveChangesAsync();
            return family;
        }

        public bool DeleteFamily(int FamilyId)
        {
            throw new NotImplementedException();
        }
    }
}
