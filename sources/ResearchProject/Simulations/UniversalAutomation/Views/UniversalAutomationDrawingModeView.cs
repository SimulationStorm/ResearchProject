using System.Collections.Generic;
using EasyBindings;

public partial class UniversalAutomationDrawingModeView : AutomationDrawingModeView<byte>, IView<UniversalAutomationDrawingModeVM>
{
    #region Fields
    private UniversalAutomationDrawingModeVM _viewModel = null!;
    
    private readonly IDictionary<RichButton, byte> _statesByButton = new Dictionary<RichButton, byte>();
    #endregion

    public void Setup(UniversalAutomationDrawingModeVM viewModel)
    {
        _viewModel = viewModel;
        base.Setup(viewModel);
    }

    protected override void SetupBrushCellStateButtons()
    {
        GenerateBrushCellStateButtons();

        TriggerBinder.OnPropertyChanged(this, _viewModel, o => o.States, GenerateBrushCellStateButtons);
        TriggerBinder.OnPropertyChanged(this, _viewModel, o => o.NamesByState, UpdateBrushCellStateButtonsText);
    }

    #region Private methods
    private void GenerateBrushCellStateButtons()
    {
        foreach (var (button, state) in _statesByButton)
            CommandBinder.Unbind(this, button);

        _statesByButton.Clear();
        BrushCellStatesBox.RemoveChildren();
        
        foreach (var state in _viewModel.States)
        {
            var button = new RichButton
            {
                Text = _viewModel.NamesByState[state],
                FocusMode = FocusModeEnum.None,
                SizeFlagsHorizontal = SizeFlags.ExpandFill
            };

            _statesByButton[button] = state;
            BrushCellStatesBox.AddChild(button);

            CommandBinder.Bind(this, button, _viewModel.SetDrawingBrushCellStateCommand, () => _statesByButton[button]);
        }
    }

    private void UpdateBrushCellStateButtonsText()
    {
        foreach (var (button, state) in _statesByButton)
            button.Text = _viewModel.NamesByState[state];
    }
    #endregion

    public override void Unsubscribe()
    {
        TriggerBinder.Unbind(this);
        CommandBinder.Unbind(this);
    }
}