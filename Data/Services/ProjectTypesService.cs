using Etaa.Models;

namespace Etaa.Data.Services
{
    public class ProjectTypesService : IProjectTypesService
    {
        private readonly ApplicationDbContext _dbContext;

        public ProjectTypesService(ApplicationDbContext applicationDbContext)
        {
            _dbContext = applicationDbContext;
        }

        public async Task AddProjectTypeAsync(ProjectTypes ProjectType)
        {
            var DomainTypeId = ProjectType.ProjectDomainTypeId;
            var GroupId = ProjectType.ProjectGroupId;
            var Result = await _dbContext.ProjectTypes.AddAsync(ProjectType);
            await _dbContext.SaveChangesAsync();
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
