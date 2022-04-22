using Etaa.Data;
using Etaa.Models;
using Microsoft.AspNetCore.Mvc;

namespace Etaa.Controllers
{
    public class ProjectTypesController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public IActionResult Index()
        {
            List<ProjectTypes> projectTypes = _dbContext.ProjectTypes.ToList();

            return View(projectTypes);
        }

        public IActionResult Create()
        {
            return View();
        }

        public async Task<IActionResult> Add(ProjectTypes projectTypes)
        {
            _dbContext.ProjectTypes.Add(projectTypes);

            await _dbContext.SaveChangesAsync();
            return View("Index");
        }
    }
}
