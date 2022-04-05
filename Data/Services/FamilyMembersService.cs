using Etaa.Models;
using Microsoft.EntityFrameworkCore;

namespace Etaa.Data.Services
{
    public class FamilyMembersService : IFamilyMembersService
    {
        private readonly ApplicationDbContext _dbContext;

        public FamilyMembersService(ApplicationDbContext applicationDbContext)
        {
            _dbContext = applicationDbContext;
        }

        public async Task<int> AddFamilyMemberAsync(IEnumerable<FamilyMember> FamilyMembers, int FamilyId)
        {
            var FamilyMembersList = new List<FamilyMember>();
            foreach (var FamilyMember in FamilyMembers)
            {
                FamilyMember.FamilyId = FamilyId;
                FamilyMembersList.Add(FamilyMember);
                await _dbContext.Set<FamilyMember>().AddRangeAsync(FamilyMembersList);
            }
            //var Result = await _dbContext.FamilyMembers.AddRangeAsync(
            //    FamilyMember.Select(FamilyMember => new FamilyMember { NameEn = FamilyMember.NameEn, NameAr = FamilyMember.NameAr,
            //    Age = FamilyMember.Age, EducationalStatusId = FamilyMember.EducationalStatusId, FamilyId = FamilyId, GenderId = FamilyMember.GenderId,
            //    JobId = FamilyMember.JobId, KinshipId = FamilyMember.KinshipId, Note = FamilyMember.Note}));
            await _dbContext.SaveChangesAsync();
            return StatusCodes.Status200OK;
        }

        public bool DeleteFamilyMember(int FamilyMemberId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<FamilyMember>> GetFamilyMembersAsync(int FamilyId)
        {
            throw new NotImplementedException();
        }

        public Task<FamilyMember> UpdateFamilyMemberAsync(int FamilyMemberId, FamilyMember FamilyMember)
        {
            throw new NotImplementedException();
        }
    }
}
