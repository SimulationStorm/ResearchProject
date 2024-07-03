using System.ComponentModel;

public interface IPanelViewModel : INotifyPropertyChanged
{
    bool IsShown { get; set; }
}