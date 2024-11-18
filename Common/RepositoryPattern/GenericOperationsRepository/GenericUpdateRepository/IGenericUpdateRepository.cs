public interface IGenericUpdateRepository<T> where T : BaseEntity
{
    void Update(T value);
    void UpdateRange(IEnumerable<T> value);
}