using Etaa.Models;

namespace Etaa.Data.Services
{
    public interface IProjectTypesAssetsService
    {
        public Task<IEnumerable<ProjectTypesAssets>> GetAllAsync(int ProjectTypeId);
        Task AddProjectTypesAssetAsync(ProjectTypesAssets ProjectTypesAssets);
        Task<ProjectTypesAssets> GetProjectTypesAssetAsync(int ProjectTypesAssetsId);
        Task<ProjectTypesAssets> UpdateProjectTypesAssetAsync(int ProjectTypesAssetsId, ProjectTypesAssets ProjectTypesAssets);
        bool DeleteProjectTypesAsset(int ProjectTypeId);
    }
}
