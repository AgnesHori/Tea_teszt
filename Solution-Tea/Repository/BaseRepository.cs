using Microsoft.EntityFrameworkCore;
using Solution_Tea.Context;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution_Tea.Repository
{

    public class BaseRepository<TEntity> where TEntity : class, new()
    {
        public BaseRepository()
        {
        }

        public virtual TEntity Create(TEntity entity)
        {
            using (AppDbContext _context = new AppDbContext())
            {
                _context.Set<TEntity>()
                        .Add(entity);
                _context.SaveChanges();

                return entity;
            }
        }

        public virtual TEntity FindByID(int id)
        {
            using (AppDbContext _context = new AppDbContext())
            {
                return _context.Set<TEntity>().Find(id);
            }
        }

        public virtual ObservableCollection<TEntity> Get(Func<TEntity, bool> predicate)
        {
            using (AppDbContext _context = new AppDbContext())
            {
                List<TEntity> items = _context.Set<TEntity>()
                                              .AsNoTracking()
                                              .Where(predicate)
                                              .ToList();

                return new ObservableCollection<TEntity>(items);
            }
        }

        public virtual ObservableCollection<TEntity> GetAll()
        {
            using (AppDbContext _context = new AppDbContext())
            {
                List<TEntity> items = _context.Set<TEntity>()
                                              .AsNoTracking()
                                              .ToList();

                return new ObservableCollection<TEntity>(items);
            }
        }

        public virtual void Remove(TEntity entity)
        {
            using (AppDbContext _context = new AppDbContext())
            {
                _context.Set<TEntity>().Remove(entity);
                _context.SaveChanges();
            }
        }

        public virtual TEntity Update(TEntity entity, int key)
        {
            using (AppDbContext _context = new AppDbContext())
            {
                TEntity toRemove = _context.Set<TEntity>().Find(key);

                if (toRemove != null && entity != null)
                {
                    _context.Entry(toRemove).State = EntityState.Detached;
                    _context.Entry(entity).State = EntityState.Modified;
                    _context.SaveChanges();
                }

                return entity;
            }
        }
    }
}
