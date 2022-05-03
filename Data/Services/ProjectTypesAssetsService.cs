using Etaa.Models;
using Microsoft.EntityFrameworkCore;

namespace Etaa.Data.Services
{
    public class ProjectTypesAssetsService : IProjectTypesAssetsService
    {
        private readonly ApplicationDbContext _dbContext;

        public ProjectTypesAssetsService(ApplicationDbContext applicationDbContext)
        {
            _dbContext = applicationDbContext;
        }

        public async Task AddProjectTypesAssetAsync(ProjectTypesAssets ProjectTypesAssets)
        {
            var Result = await _dbContext.ProjectTypesAssets.AddAsync(ProjectTypesAssets);
            await _dbContext.SaveChangesAsync();
        }

        public bool DeleteProjectTypesAsset(int ProjectTypeId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ProjectTypesAssets>> GetAllAsync(int ProjectTypeId)
        {
            var Result = await _dbContext.ProjectTypesAssets.Where(ProjectTypesAssets => ProjectTypesAssets.ProjectTypeId == ProjectTypeId).OrderBy(ProjectTypesAsset => ProjectTypesAsset.NameAr).ToListAsync();
            return Result;
        }

        public async Task<ProjectTypesAssets> GetProjectTypesAssetAsync(int ProjectTypesAssetsId)
        {
            var Result = await _dbContext.ProjectTypesAssets.FirstOrDefaultAsync(ProjectTypesAsset => ProjectTypesAsset.ProjectTypesAssetsId == ProjectTypesAssetsId);
            return Result;
        }

        public Task<ProjectTypesAssets> UpdateProjectTypesAssetAsync(int ProjectTypesAssetsId, ProjectTypesAssets ProjectTypesAssets)
        {
            throw new NotImplementedException();
        }
    }
}
