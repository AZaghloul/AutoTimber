using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace Algorithm.DB.DAL
{
    public class Repository<TEntity> where TEntity : class
    {
        internal DbContext _ctx;
        internal DbSet<TEntity> _set;
        public Repository(DbContext ctx)
        {
            _ctx = ctx;
            _set = _ctx.Set<TEntity>();
        }

        public IEnumerable<TEntity> All()
        {
            return _set.AsNoTracking().ToList();
        }
        public IEnumerable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate)
        {
            return _set
                .Where(predicate).ToList();
        }
        public TEntity FindById(params object[] key)
        {
            return _set.Find(key);
            //return _set.AsNoTracking().SingleOrDefault(e => e.Id == key);
        }

        public void Insert(TEntity entity)
        {
            _set.Add(entity);
        }
        public void Update(TEntity entity)
        {
            _set.Attach(entity);
            _ctx.Entry(entity).State = EntityState.Modified;
        }
        public void Delete(int Id)
        {
            var entity = _set.Find(Id);
            _set.Remove(entity);
        }
    }
}
