using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Godot;

public class UniversalAutomationStateVM : ObservableObject
{
    #region Properties
    public byte State { get; }

    public string Name
    {
        get => _presentationModel.NamesByState[State];
        set
        {
            _presentationModel.SetStateName(State, value);
            OnPropertyChanged();
        }
    }

    public Color Color
    {
        get => _presentationModel.ColorsByState[State];
        set
        {
            _presentationModel.SetStateColor(State, value);
            OnPropertyChanged();
        }
    }

    public IRelayCommand<UniversalAutomationStateVM> DeleteCommand { get; }
    #endregion

    private readonly UniversalAutomationPresentationModel _presentationModel;

    public UniversalAutomationStateVM
    (
        byte state,
        UniversalAutomationPresentationModel presentationModel,
        IRelayCommand<UniversalAutomationStateVM> deleteCommand)
    {
        _presentationModel = presentationModel;

        State = state;

        DeleteCommand = deleteCommand;
    }
}