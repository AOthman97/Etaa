﻿using Etaa.Models;

namespace Etaa.Data.Services
{
    public interface IProjectTypesService
    {
        Task<int> AddProjectTypeAsync(IEnumerable<ProjectTypes> ProjectType);
        Task<IEnumerable<ProjectTypes>> GetProjectTypeAsync(int ProjectTypeId);
        Task<ProjectTypes> UpdateProjectTypeAsync(int ProjectTypeId, ProjectTypes ProjectTypes);
        bool DeleteProjectType(int ProjectTypeId);
    }
}
