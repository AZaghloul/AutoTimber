using System.Data.Entity;

namespace Algorithm.MVC.DAL
{
    public class UnitOfWork
    {
        DbContext _ctx;


      

      

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
