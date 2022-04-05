using Etaa.Models;

namespace Etaa.Data.Services
{
    public interface IContributorsService
    {
        public Task<IEnumerable<Contributor>> GetAllAsync();
        Task<Contributor> GetContributorAsync(int ContributorId);
        Task AddContributorAsync(Contributor contributor);
        Task<Contributor> UpdateContributorAsync(int ContributorId, Contributor contributor);
        bool DeleteContributor(int ContributorId);
    }
}
