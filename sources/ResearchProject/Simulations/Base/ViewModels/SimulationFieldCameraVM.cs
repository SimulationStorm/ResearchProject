using CommunityToolkit.Mvvm.ComponentModel;
using EasyBindings;
using EasyBindings.Interfaces;
using Godot;

public class SimulationFieldCameraVM : ObservableObject, IUnsubscribe
{
    #region Properties
    public Vector2I ScreenSize => _fieldStateModel.ScreenSize;

    public double ViewScale
    {
        get => _fieldStateModel.ViewScale;
        set => _fieldStateModel.ViewScale = value;
    }
    #endregion

    private readonly FieldStateModel _fieldStateModel;

    public SimulationFieldCameraVM(FieldStateModel fieldStateModel)
    {
        _fieldStateModel = fieldStateModel;
        TriggerBinder.OnPropertyChanged(this, _fieldStateModel, o => o.ViewScale, () => OnPropertyChanged(nameof(ViewScale)));
    }

    public void Unsubscribe() => TriggerBinder.Unbind(this);
}