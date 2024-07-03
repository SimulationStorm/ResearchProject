using System.ComponentModel;

/// <summary>
/// Notifies clients that a state has advanced.
/// </summary>
public interface INotifyAdvanced : INotifyPropertyChanged
{
    /// <summary>
    /// The property that will be passed to the <see cref='PropertyChangedEventArgs'/> constructor.
    /// </summary>
    object? Advanced { get; }
}