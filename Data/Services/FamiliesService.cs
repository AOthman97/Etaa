using Etaa.Models;

namespace Etaa.Data.Services
{
    public class FamiliesService : IFamiliesService
    {
        private readonly ApplicationDbContext _dbContext;

        public FamiliesService(ApplicationDbContext applicationDbContext)
        {
            _dbContext = applicationDbContext;
        }

        public Task AddFamilyAsync(Family family)
        {
            throw new NotImplementedException();
        }

        public bool DeleteFamily(int FamilyId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Family>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Family> GetFamilyAsync(int FamilyId)
        {
            throw new NotImplementedException();
        }

        public Task<Family> UpdateFamilyAsync(int FamilyId, Family family)
        {
            throw new NotImplementedException();
        }
    }
}
