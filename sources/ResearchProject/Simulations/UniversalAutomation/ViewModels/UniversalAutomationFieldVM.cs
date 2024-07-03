using Godot;
using System.Collections.Generic;

public class UniversalAutomationFieldVM : SimulationFieldVM
{
    #region Properties
    public IGetCell<byte> FieldCellGetter { get; }

    public IReadOnlyDictionary<byte, Color> CellColorsByStateNumber { get; }
    #endregion

    public UniversalAutomationFieldVM
    (
        SimulationManagerModel simulationManagerModel,
        FieldStateModel fieldStateModel,
        UniversalAutomationModel automationModel,
        UniversalAutomationPresentationModel presentationModel
    )
    : base(fieldStateModel, simulationManagerModel, automationModel)
    {
        FieldCellGetter = automationModel.FieldCellGetter;
        CellColorsByStateNumber = presentationModel.ColorsByState;

        // There is no handlers for state added and removed, because of at the moment of
        // adding/deleting state, there is no cells in this state on field because presentation model updated first, then automation
        // (which will generate state changed)
        presentationModel.StateColorChanged += _ => NotifyStateChanged();
    }
}