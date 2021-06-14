using Assets.Scripts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class OrderedItem<T>
{
    private int _order = default;
    private T _execute = default;

    public int Order => _order;
    public T Item => _execute;

    public OrderedItem(int order, T execute)
    {
        _order = order;
        _execute = execute;
    }
}
