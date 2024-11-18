public interface IGenericDeleteRepository<T> where T:BaseEntity
{
    void Delete(T value);
    void DeleteRange(IEnumerable<T> value);
}