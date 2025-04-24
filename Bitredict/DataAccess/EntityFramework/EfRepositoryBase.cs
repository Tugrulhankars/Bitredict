using Bitredict.DataAccess.Abstract;
using Microsoft.EntityFrameworkCore;

namespace Bitredict.DataAccess.EntityFramework;

public class EfRepositoryBase<TEntity, TContext> : IRepository<TEntity>
    where TEntity : class
    where TContext : DbContext
{
    private readonly TContext _context;
    public EfRepositoryBase(TContext context)
    {
        _context = context;
    }
    public async Task Add(TEntity entity)
    {
        await _context.AddAsync(entity);
        await _context.SaveChangesAsync();
    }
    //public void Delete(TEntity entity)
    //{
    //    using (TContext context = new TContext())
    //    {
    //        var deletedEntity = context.Entry(entity);
    //        deletedEntity.State = EntityState.Deleted;
    //        context.SaveChanges();
    //    }
    //}
    public async Task Delete(TEntity entity)
    {
        //var deletedEntity = _context.Entry(entity);
        //deletedEntity.State = EntityState.Deleted;
        _context.Set<TEntity>().Remove(entity);
        _context.SaveChanges();
    }

    public async Task<List<TEntity>> GetAll()
    {
        var result = await _context.Set<TEntity>().ToListAsync();
        return result;
    }

    public async Task Update(TEntity entity)
    {
        //_context.Update(entity);
        //await _context.SaveChangesAsync();
        //return entity;

        var updatedEntity = _context.Entry(entity);
        updatedEntity.State = EntityState.Modified;
        _context.SaveChanges();

    }
}

