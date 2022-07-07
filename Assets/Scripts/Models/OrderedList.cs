using System;
using System.Collections.Generic;

public class OrderedList<T> : List<OrderedItem<T>> where T : class
{
    private readonly int _defaultItemOrder = default;
    public new T this[int index] => base[index].Item;

    public OrderedList(int defaultItemOrder = 100) : base()
    {
        _defaultItemOrder = defaultItemOrder;
    }

    public new void Add(OrderedItem<T> orderedItem)
    {
        if (Count == 0)
        {
            base.Add(orderedItem);
            return;
        }

        for (int i = 0; i < Count; i++)
        {
            if (orderedItem.Order < base[i].Order)
            {
                Insert(i, orderedItem);
                return;
            }
        }

        base.Add(orderedItem);
    }

    public void Add(int order, T item) => Add(new OrderedItem<T>(order, item));
    public void Add(T item) => Add(_defaultItemOrder, item);

    public void Remove(T item)
    {
        for (int i = 0; i < Count; i++)
        {
            if (EqualityComparer<T>.Default.Equals(item, this[i]))
            //if (item == this[i])
            {
                RemoveAt(i);
            }
        }
    }

    public new IEnumerator<T> GetEnumerator()
    {
        var resultList = new List<T>();
        for (int i = 0; i < Count; i++)
        {
            resultList.Add(this[i]);
        }
        return resultList.GetEnumerator();
    }

    public void ForEach(Action<T> action)
    {
        ForEach(orderedItem => action(orderedItem.Item));
    }
}
