public interface IGenericAddRepository<T> where T:BaseEntity
{
    Task AddAsync(T value);
    Task AddRangeAsync(IEnumerable<T> values);
}