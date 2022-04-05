using Etaa.Models;
using Microsoft.EntityFrameworkCore;

namespace Etaa.Data.Services
{
    public class ContributorsService : IContributorsService
    {
        private readonly ApplicationDbContext _dbContext;

        public ContributorsService(ApplicationDbContext applicationDbContext)
        {
            _dbContext = applicationDbContext;
        }

        public async Task AddContributorAsync(Contributor contributor)
        {
            var Result = await _dbContext.Contributors.AddAsync(contributor);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Contributor>> GetAllAsync()
        {
            var Result = await _dbContext.Contributors.OrderBy(Contributor => Contributor.NameAr).ToListAsync();
            return Result;
        }

        public async Task<Contributor> GetContributorAsync(int ContributorId)
        {
            var Result = await _dbContext.Contributors.FirstOrDefaultAsync(Contributor => Contributor.ContributorId == ContributorId);
            return Result;
        }

        public async Task<Contributor> UpdateContributorAsync(int ContributorId, Contributor contributor)
        {
            _dbContext.Update(contributor);
            await _dbContext.SaveChangesAsync();
            return contributor;
        }

        public bool DeleteContributor(int ContributorId)
        {
            throw new NotImplementedException();
        }
    }
}
