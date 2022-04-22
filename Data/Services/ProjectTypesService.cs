using Etaa.Models;

namespace Etaa.Data.Services
{
    public class ProjectTypesService : IProjectTypesService
    {
        public Task<int> AddProjectTypeAsync(IEnumerable<ProjectTypes> ProjectType)
        {
            throw new NotImplementedException();
        }

        public bool DeleteProjectType(int ProjectTypeId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ProjectTypes>> GetProjectTypeAsync(int ProjectTypeId)
        {
            throw new NotImplementedException();
        }

        public Task<ProjectTypes> UpdateProjectTypeAsync(int ProjectTypeId, ProjectTypes ProjectTypes)
        {
            throw new NotImplementedException();
        }
    }
}
