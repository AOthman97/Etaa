using Etaa.Models;

namespace Etaa.Data.Services
{
    public interface IProjectTypesService
    {
        public Task<IEnumerable<ProjectTypes>> GetAllAsync();
        Task AddProjectTypeAsync(ProjectTypes ProjectType);
        Task<ProjectTypes> GetProjectTypeAsync(int ProjectTypeId);
        Task<ProjectTypes> UpdateProjectTypeAsync(int ProjectTypeId, ProjectTypes ProjectTypes);
        bool DeleteProjectType(int ProjectTypeId);
    }
}
