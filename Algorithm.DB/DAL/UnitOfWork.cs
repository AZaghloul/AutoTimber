using Algorithm.DB.Managers;
using System.Data.Entity;

namespace Algorithm.MVC.DAL
{
    public class UnitOfWork
    {
        DbContext _ctx;

        public ProjectsManager Projects
        {
            get { return new ProjectsManager(_ctx); }
        }

        public UsersManagers Users
        {
            get { return new UsersManagers(_ctx); }
        }

        public DesignOptionsManager DesignOptions
        {
            get { return new DesignOptionsManager(_ctx); }
        }

        public UnitOfWork(DbContext dbContext)
        {
            _ctx = dbContext;
        }

        public void SaveChanges()
        {
            _ctx.SaveChanges();
        }
    }
}
