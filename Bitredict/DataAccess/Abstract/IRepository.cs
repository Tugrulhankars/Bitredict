namespace Bitredict.DataAccess.Abstract;

public interface IRepository<TEntity>
    where TEntity : class
{
    public Task Add(TEntity entity);
    public Task Update(TEntity entity);
    public Task<List<TEntity>> GetAll();
    public Task Delete(TEntity entity);
}
