public class UnitOfWork<T> : IUnitOfWork<T> where T : BaseEntity
{
    private readonly ApplicationContext _context;

    public UnitOfWork(
        IGenericUpdateRepository<T> updateRepository,
        IGenericAddRepository<T> addRepository,
        IGenericFindRepository<T> findRepository,
        IGenericDeleteRepository<T> deleteRepository,
        ApplicationContext context)
    {
        AddRepository = addRepository;
        DeleteRepository = deleteRepository;
        FindRepository = findRepository;
        UpdateRepository = updateRepository;
        _context = context ?? throw new ArgumentNullException(nameof(context)); 
    }

    public IGenericUpdateRepository<T> UpdateRepository { get; }
    public IGenericDeleteRepository<T> DeleteRepository { get; }
    public IGenericFindRepository<T> FindRepository { get; }
    public IGenericAddRepository<T> AddRepository { get; }

    public void Dispose()
    {
        _context.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await _context.DisposeAsync();
    }

    public async Task<int> Complete()
    {
        return await _context.SaveChangesAsync();
    }
}
