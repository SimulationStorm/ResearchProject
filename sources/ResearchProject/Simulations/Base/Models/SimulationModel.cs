using CommunityToolkit.Mvvm.ComponentModel;
using EasyBindings.Interfaces;
using Godot;

public abstract class SimulationModel : ObservableObject, INotifyStateChanged, INotifyWasReset, INotifyAdvanced, IUnsubscribe
{
    #region Properties
    public object? State { get; }

    public object? Advanced { get; }

    public object? WasReset { get; }
    #endregion

    #region Public methods
    public virtual bool CanAdvance() => true;

    public void Advance()
    {
        DoAdvance();
        OnPropertyChanged(nameof(Advanced));
    }

    public void Reset(Vector2I? newFieldSize = null)
    {
        DoReset(newFieldSize);
        OnPropertyChanged(nameof(WasReset));
    }
    #endregion

    #region Protected methods
    protected abstract void DoAdvance();

    // TODO: Rename to reset field ?
    // Это сброс и всех настроек к их исходным значениям ? Или нет ? 
    protected abstract void DoReset(Vector2I? newFieldSize);

    protected void NotifyStateChanged() => OnPropertyChanged(nameof(State));
    #endregion

    public virtual void Unsubscribe() { }
}