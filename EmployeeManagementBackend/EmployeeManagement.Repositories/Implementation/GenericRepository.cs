using System.Linq.Expressions;
using EmployeeManagement.Entities.Data;
using EmployeeManagement.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Repositories.Implementation;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    protected readonly EmpManagementContext _context;
    protected readonly DbSet<T> _dbSet;

    public GenericRepository(EmpManagementContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    // GET All
    public async Task<IEnumerable<T>> GetAllAsync(
        Expression<Func<T, bool>>? filter = null,
        Expression<Func<T, object>>? orderBy = null,
        Func<IQueryable<T>, IQueryable<T>>? include = null)
    {
        IQueryable<T> query = _dbSet;

        // Apply filter
        if (filter != null)
        {
            query = query.Where(filter);
        }

        // Apply orderBy
        if (orderBy != null)
        {
            query = query.OrderBy(orderBy);
        }

        // Apply include
        if (include != null)
        {
            query = include(query);
        }

        return await query.ToListAsync();
    }

    // GET ENTITY BY ID
    public virtual async Task<T?> GetByIdAsync(int id) => await _dbSet.FindAsync(id);
    
    // Add Async
    public virtual async Task<T?> AddAsync(T entity)
    {
        try
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred during AddAsync: {ex.Message}");
            if (ex.InnerException != null)
            {
                Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
            }
            throw; 
        }
    }

    // UPDTAE ENTITY
    public virtual async Task<T?> UpdateAsync(T entity, Func<T, bool> checkUniquePredicate = null) // Changed return type to Task<T?>
    {
        try
        {
            if (checkUniquePredicate != null && _dbSet.Any(checkUniquePredicate))
            {
                return null; // Unique constraint violation: return null to indicate failure
            }

            _dbSet.Update(entity);
            await _context.SaveChangesAsync(); // Save changes immediately

            return entity; // Return the updated entity
        }
        catch (DbUpdateException ex)
        {
            Console.Error.WriteLine($"Database update error in UpdateAsync: {ex.Message}");
            if (ex.InnerException != null)
            {
                Console.Error.WriteLine($"Inner exception: {ex.InnerException.Message}");
            }
            throw new InvalidOperationException("A database error occurred while updating the entity.", ex);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"An unexpected error occurred in UpdateAsync: {ex.Message}");
            throw new InvalidOperationException("An unexpected error occurred while updating the entity.", ex);
        }
    }

    // DELETE ENTITTY
    public virtual async Task DeleteAsync(T entity)
    {
        try
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred during AddAsync: {ex.Message}");
            if (ex.InnerException != null)
            {
                Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
            }
            throw; 
        }
    }
}
