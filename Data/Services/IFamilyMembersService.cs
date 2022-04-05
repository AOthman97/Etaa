using Etaa.Models;

namespace Etaa.Data.Services
{
    public interface IFamilyMembersService
    {
        Task<int> AddFamilyMemberAsync(IEnumerable<FamilyMember> FamilyMember, int FamilyId);
        Task<IEnumerable<FamilyMember>> GetFamilyMembersAsync(int FamilyId);
        Task<FamilyMember> UpdateFamilyMemberAsync(int FamilyMemberId, FamilyMember FamilyMember);
        bool DeleteFamilyMember(int FamilyMemberId);
    }
}
