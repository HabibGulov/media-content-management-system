
public class GenericDeleteRepository<T>(ApplicationContext context) : IGenericDeleteRepository<T> where T : BaseEntity
{
    public void Delete(T value)
    {
        context.Remove(value);
    }

    public void DeleteRange(IEnumerable<T> values)
    {
        context.RemoveRange(values);
    }
}
