using EasyBindings.Interfaces;
using Godot;

public abstract partial class SimulationView : Node, IUnsubscribe
{
    public virtual void Unsubscribe() { }
}