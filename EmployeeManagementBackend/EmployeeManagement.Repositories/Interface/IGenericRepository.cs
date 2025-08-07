using System.Linq.Expressions;

namespace EmployeeManagement.Repositories.Interface;

public interface IGenericRepository<T> where T: class
{
    public Task<IEnumerable<T>> GetAllAsync(
        Expression<Func<T, bool>>? filter = null,
        Expression<Func<T, object>>? orderBy = null,
        Func<IQueryable<T>, IQueryable<T>>? include = null);

    Task<(IEnumerable<T> records, int totalRecord, int pageIndex,int pageSize)> GetPagedRecords(
        int pageSize,
        int pageIndex,
        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
        Expression<Func<T, bool>>? filter = null,
        Func<IQueryable<T>, IQueryable<T>>? include = null
    );

    // GET ENTITY BY ID
    Task<T?> GetFirstOrDefaultAsync(Expression<Func<T,bool>> filter,Func<IQueryable<T>, IQueryable<T>>? include);
    public Task<T?> GetByIdAsync(int id);
    public Task AddAsync(T entity);
    Task AddRangeAsync(IEnumerable<T> entities);
    public Task<T?> UpdateAsync(T entity, Func<T, bool> checkUniquePredicate = null);
    public Task DeleteAsync(T entity);
}
