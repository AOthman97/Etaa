using Etaa.Models;

namespace Etaa.Data.Services
{
    public interface IFamiliesService
    {
        public Task<IEnumerable<Family>> GetAllAsync();
        Task<Family> GetFamilyAsync(int FamilyId);
        Task AddFamilyAsync(Family family);
        Task<Family> UpdateFamilyAsync(int FamilyId, Family family);
        bool DeleteFamily(int FamilyId);
    }
}
