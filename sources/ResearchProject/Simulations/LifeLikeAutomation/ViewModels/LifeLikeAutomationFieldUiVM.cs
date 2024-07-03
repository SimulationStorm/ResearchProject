using CommunityToolkit.Mvvm.Input;
using Godot;

public partial class LifeLikeAutomationFieldUiVM : AutomationFieldUiVM<LifeLikeAutomationCellState>
{
    [RelayCommand(CanExecute = nameof(CanPlacePattern))]
    private void PlacePattern(Vector2I position) => _automationModel.PlacePattern(position);
    private bool CanPlacePattern() => _automationModel.Pattern is not null;

    private readonly LifeLikeAutomationModel _automationModel;

    public LifeLikeAutomationFieldUiVM
    (
        FieldStateModel fieldStateModel,
        LifeLikeAutomationModel automationModel,
        LifeLikeAutomationPresentationModel presentationModel
    )
    : base(fieldStateModel, automationModel, presentationModel)
    {
        _automationModel = automationModel;
    }
}