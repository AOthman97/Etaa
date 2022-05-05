using Etaa.Models;

namespace Etaa.Data.Services
{
    public interface IProjectsService
    {
        public Task<IEnumerable<Projects>> GetAllAsync();
        Task<Projects> GetProjectAsync(int ProjectId);
        Task AddProjectAsync(Projects Project);
        Task<Projects> UpdateProjectAsync(int ProjectId, Projects Project);
        bool DeleteProject(int ProjectId);
    }
}
