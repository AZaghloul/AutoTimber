using Algorithm.DB.DAL;
using Algorithm.DB.Models;
using System.Data.Entity;

namespace Algorithm.DB.Managers
{
    public class ProjectsManager:Repository<Project>
    {

        public ProjectsManager(DbContext dbContext ):base( dbContext)
        {

        }
    }
}