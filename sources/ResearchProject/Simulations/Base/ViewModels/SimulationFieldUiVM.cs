using Godot;
using CommunityToolkit.Mvvm.ComponentModel;
using EasyBindings.Interfaces;

public abstract class SimulationFieldUiVM : ObservableObject, IUnsubscribe
{
    #region Properties
    public SimulationFieldCameraVM CameraVM { get; }

    public Vector2I FieldSize => _fieldStateModel.FieldSize;

    public float CellSize => (float)_fieldStateModel.CellSize;
    #endregion

    private readonly FieldStateModel _fieldStateModel;

    protected SimulationFieldUiVM(FieldStateModel fieldStateModel)
    {
        _fieldStateModel = fieldStateModel;

        CameraVM = new(_fieldStateModel);
    }
    
    public virtual void Unsubscribe() => CameraVM.Unsubscribe();
}