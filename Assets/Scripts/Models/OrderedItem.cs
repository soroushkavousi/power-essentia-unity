public class OrderedItem<T>
{
    private readonly int _order = default;
    private readonly T _execute = default;

    public int Order => _order;
    public T Item => _execute;

    public OrderedItem(int order, T execute)
    {
        _order = order;
        _execute = execute;
    }
}
