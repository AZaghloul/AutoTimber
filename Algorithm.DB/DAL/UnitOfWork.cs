using Algorithm.DB;
using Algorithm.DB.Managers;
using System.Data.Entity;
using System.Runtime.InteropServices;

namespace Algorithm.MVC.DAL
{
    public class UnitOfWork
    {
        DbContext _ctx= new AlgorithmDB();

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

        public UnitOfWork([Optional]DbContext dbContext)
        {
            if (dbContext==null)
            {
                return;
            }
            _ctx = dbContext;
        }

        public void SaveChanges()
        {
            _ctx.SaveChanges();
        }
    }
}
