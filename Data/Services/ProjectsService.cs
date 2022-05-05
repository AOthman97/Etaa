using Etaa.Models;
using Microsoft.EntityFrameworkCore;

namespace Etaa.Data.Services
{
    public class ProjectsService : IProjectsService
    {
        private readonly ApplicationDbContext _dbContext;

        public ProjectsService(ApplicationDbContext applicationDbContext)
        {
            _dbContext = applicationDbContext;
        }

        public async Task AddProjectAsync(Projects Project)
        {
            var Result = await _dbContext.Projects.AddAsync(Project);
            await _dbContext.SaveChangesAsync();
        }

        public bool DeleteProject(int ProjectId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Projects>> GetAllAsync()
        {
            var Result = await _dbContext.Projects.ToListAsync();
            return Result;
        }

        public async Task<Projects> GetProjectAsync(int ProjectId)
        {
            var Result = await _dbContext.Projects.FirstOrDefaultAsync(Project => Project.ProjectId == ProjectId);
            return Result;
        }

        public async Task<Projects> UpdateProjectAsync(int ProjectId, Projects Project)
        {
            _dbContext.Update(Project);
            await _dbContext.SaveChangesAsync();
            return Project;
        }
    }
}
