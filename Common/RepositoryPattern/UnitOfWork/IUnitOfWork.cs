public interface IUnitOfWork<T> : IDisposable, IAsyncDisposable where T:BaseEntity
{
    public IGenericUpdateRepository<T> UpdateRepository { get; }
    public IGenericDeleteRepository<T> DeleteRepository { get; }
    public IGenericFindRepository<T> FindRepository { get; }
    public IGenericAddRepository<T> AddRepository { get; }

    Task<int> Complete();
}