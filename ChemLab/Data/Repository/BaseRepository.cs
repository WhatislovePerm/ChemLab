using Microsoft.EntityFrameworkCore;
using ChemLab.Data.DataContext;
using ChemLab.Data.Repository.Interfaces;

namespace ChemLab.Data.Repository
{
    public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        protected readonly ApplicationDbContext db;

        public BaseRepository(ApplicationDbContext context) =>
            db = context;

        public virtual async Task Add(TEntity obj)
        {
            db.Add(obj);
            await db.SaveChangesAsync();
        }

        public virtual async Task<IEnumerable<TEntity>> GetAll() =>
            await db.Set<TEntity>().ToListAsync();

        public virtual async Task<TEntity?> GetById(int? id) =>
            await db.Set<TEntity>().FindAsync(id);

        public virtual async Task Remove(TEntity obj)
        {
            db.Set<TEntity>().Remove(obj);
            await db.SaveChangesAsync();
        }

        public virtual async Task Update(TEntity obj)
        {
            db.Entry(obj).State = EntityState.Modified;
            await db.SaveChangesAsync();
        }

        public void Dispose() =>
            db.Dispose();

    }
}

