using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Chain of Responsibility + Observer pattern.
public abstract class MaxHealthModifier : ISubject
{
    public List<IObserver> Observers { get; private set; } = new List<IObserver>();
    public MaxHealthModifier NextModifier { get; set; }
    public abstract MaxHealthModifierType Type { get; }

    public abstract float Apply(float value);
    public void Attach(IObserver observer) => Observers.Add(observer);
    public void Detach(IObserver observer) => Observers.Remove(observer);
    public void Notify() => Observers.ForEach(observer => observer.Update(this));
}
