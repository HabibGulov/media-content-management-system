public class GenericUpdateRepository<T>(ApplicationContext context) : IGenericUpdateRepository<T> where T : BaseEntity
{
    public void Update(T value)
    {
        context.Set<T>().Update(value);
    }

    public void UpdateRange(IEnumerable<T> value)
    {
        context.Set<T>().UpdateRange(value);
    }
}