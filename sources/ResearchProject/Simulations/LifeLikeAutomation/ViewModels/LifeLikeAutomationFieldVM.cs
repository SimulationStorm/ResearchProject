using EasyBindings;
using Godot;
using System.Collections.Generic;

public class LifeLikeAutomationFieldVM : SimulationFieldVM
{
    #region Properties
    public LifeLikeAutomationAlgorithm Algorithm => _automationModel.Algorithm;

    public IReadOnlyDictionary<LifeLikeAutomationCellState, Color> CellColorsByState => _presentationModel.CellColorsByState;
    #endregion

    #region Fields
    private readonly LifeLikeAutomationModel _automationModel;
    
    private readonly LifeLikeAutomationPresentationModel _presentationModel;
    #endregion

    public LifeLikeAutomationFieldVM
    (
        SimulationManagerModel simulationManagerModel,
        FieldStateModel fieldStateModel,
        LifeLikeAutomationModel automationModel,
        LifeLikeAutomationPresentationModel presentationModel
    )
    : base(fieldStateModel, simulationManagerModel, automationModel)
    {
        _automationModel = automationModel;
        _presentationModel = presentationModel;

        TriggerBinder.OnPropertyChanged(this, _presentationModel, o => o.CellColorsByState, NotifyStateChanged);
    }

    public override void Unsubscribe()
    {
        base.Unsubscribe();
        TriggerBinder.Unbind(this);
    }
}