using EasyBindings;
using System.Collections.Generic;

public class UniversalAutomationDrawingModeVM : AutomationDrawingModeVM<byte>
{
    #region Properties
    public IReadOnlySet<byte> States => _automationModel.States;

    public IReadOnlyDictionary<byte, string> NamesByState => _presentationModel.NamesByState;
    #endregion

    #region Fields
    private readonly UniversalAutomationModel _automationModel;

    private readonly UniversalAutomationPresentationModel _presentationModel;
    #endregion

    public UniversalAutomationDrawingModeVM
    (
        UniversalAutomationModel automationModel,
        UniversalAutomationPresentationModel presentationModel
    )
    : base(presentationModel)
    {
        _automationModel = automationModel;
        TriggerBinder.OnPropertyChanged(this, _automationModel, o => o.States, () => OnPropertyChanged(nameof(States)));

        _presentationModel = presentationModel;
        _presentationModel.StateNameChanged += _ => OnPropertyChanged(nameof(NamesByState));
    }

    public override void Unsubscribe()
    {
        base.Unsubscribe();
        TriggerBinder.Unbind(this);
    }
}