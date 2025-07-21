using System.Linq.Expressions;

namespace EmployeeManagement.Repositories.Interface;

public interface IGenericRepository<T> where T: class
{
    public Task<IEnumerable<T>> GetAllAsync(
        Expression<Func<T, bool>>? filter = null,
        Expression<Func<T, object>>? orderBy = null,
        Func<IQueryable<T>, IQueryable<T>>? include = null);
    // GET ENTITY BY ID
    public Task<T?> GetByIdAsync(int id);
    public Task<T?> AddAsync(T entity);
    public Task<T?> UpdateAsync(T entity, Func<T, bool> checkUniquePredicate = null);
    public Task DeleteAsync(T entity);
}
