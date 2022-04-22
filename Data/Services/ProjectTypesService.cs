using Etaa.Models;
using Microsoft.EntityFrameworkCore;

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

        public async Task<IEnumerable<ProjectTypes>> GetAllAsync()
        {
            var Result = await _dbContext.ProjectTypes.OrderBy(ProjectType => ProjectType.NameAr).ToListAsync();
            return Result;
        }

        public async Task<ProjectTypes> GetProjectTypeAsync(int ProjectTypeId)
        {
            var Result = await _dbContext.ProjectTypes.FirstOrDefaultAsync(ProjectType => ProjectType.ProjectTypeId == ProjectTypeId);
            return Result;
        }

        public Task<ProjectTypes> UpdateProjectTypeAsync(int ProjectTypeId, ProjectTypes ProjectTypes)
        {
            throw new NotImplementedException();
        }
    }
}
