using System.ComponentModel;

/// <summary>
/// Notifies clients that a state has reset.
/// </summary>
public interface INotifyWasReset : INotifyPropertyChanged
{
    /// <summary>
    /// The property that will be passed to the <see cref='PropertyChangedEventArgs'/> constructor.
    /// </summary>
    object? WasReset { get; }
}