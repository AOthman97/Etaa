using Etaa.Models;

namespace Etaa.Data.Services
{
    public class ContributorsService : IContributorsService
    {
        public Task AddContributorAsync(Contributor contributor)
        {
            throw new NotImplementedException();
        }

        public bool DeleteContributor(int ContributorId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Contributor>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Contributor> GetContributorAsync(int ContributorId)
        {
            throw new NotImplementedException();
        }

        public Task<Contributor> UpdateContributorAsync(int ContributorId, Contributor contributor)
        {
            throw new NotImplementedException();
        }
    }
}
