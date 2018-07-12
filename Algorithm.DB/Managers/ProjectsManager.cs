using AutoTimber.DB.DAL;
using AutoTimber.DB.Models;
using System.Data.Entity;

namespace AutoTimber.DB.Managers
{
    public class ProjectsManager:Repository<Project>
    {

        public ProjectsManager(DbContext dbContext ):base( dbContext)
        {

        }
    }
}