using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

public class GenericFindRepository<T>(ApplicationContext context) : IGenericFindRepository<T> where T : BaseEntity
{
    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> expression)
    {
        return await context.Set<T>().Where(x => !x.IsDeleted).Where(expression).ToListAsync();
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await context.Set<T>().Where(x => !x.IsDeleted).ToListAsync();

    }

    public async Task<T?> GetByIdAsync(Guid id)
    {
        return await context.Set<T>().Where(x => !x.IsDeleted).FirstOrDefaultAsync(x => x.Id == id);
    }
}