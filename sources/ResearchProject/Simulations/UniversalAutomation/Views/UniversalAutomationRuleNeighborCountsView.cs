using EasyBindings;
using EasyBindings.Interfaces;
using Godot;
using System.Collections.Generic;
using System.Linq;

public partial class UniversalAutomationRuleNeighborCountsView : Control, IView<UniversalAutomationRuleNeighborCountsVM>
{
    private UniversalAutomationRuleNeighborCountsVM _viewModel = null!;

    public void Setup(UniversalAutomationRuleNeighborCountsVM viewModel)
    {
        _viewModel = viewModel;
        SetupNeighborCountsBox();
    }

    #region Neighbor counts box
    [Export] public NodePath NeighborCountsBoxPath { get; set; } = null!;
    private Container _neighborCountsBox = null!;

    private readonly IDictionary<int, ToggleButton> _neighborButtonsByCount = new Dictionary<int, ToggleButton>();

    private void SetupNeighborCountsBox()
    {
        _neighborCountsBox = GetNode<Container>(NeighborCountsBoxPath);

        GenerateNeighborCountButtons();
        UpdateNeighborCountButtons();

        TriggerBinder.OnPropertyChanged(this, _viewModel, o => o.NeighborhoodRadius, GenerateNeighborCountButtons);
        TriggerBinder.OnPropertyChanged(this, _viewModel, o => o.NeighborhoodState, UpdateNeighborCountButtons);
    }

    private void GenerateNeighborCountButtons()
    {
        foreach (var (_, button) in _neighborButtonsByCount)
            TriggerBinder.UnbindPropertyChanged(this, button, o => o.IsToggled);

        _neighborButtonsByCount.Clear();
        _neighborCountsBox.RemoveChildren();

        UniversalAutomationNeighborhood.IterateOverNeighborCounts(_viewModel.NeighborhoodRadius, neighborCount =>
        {
            var button = new ToggleButton
            {
                Text = $"{neighborCount}",
                CustomMinimumSize = new(28, 28),
                SizeFlagsHorizontal = SizeFlags.ExpandFill,
                FocusMode = FocusModeEnum.None,
                //IsToggled = _viewModel.IsNeighborCountAdded(neighborCount)
            };
            button.AddThemeFontSizeOverride("font_size", 12);

            _neighborCountsBox.AddChild(button);

            _neighborButtonsByCount[neighborCount] = button;
            TriggerBinder.OnPropertyChanged(this, button, o => o.IsToggled, OnNeighborCountButtonIsToggledChanged);
        });
    }

    private void OnNeighborCountButtonIsToggledChanged(ToggleButton button, bool isToggled)
    {
        var neighborCount = _neighborButtonsByCount.First(kv => kv.Value == button).Key;

        if (isToggled)
            _viewModel.AddNeighborCount(neighborCount);
        else
            _viewModel.RemoveNeighborCount(neighborCount);

        UpdateButtonSelfModulate(button);
    }

    private void UpdateNeighborCountButtons()
    {
        var maxNeighborCount = _viewModel.MaxNeighborCount;

        foreach (var (neighborCount, button) in _neighborButtonsByCount)
        {
            var isAllowedNeighborCount = neighborCount <= maxNeighborCount;

            if (!isAllowedNeighborCount)
            {
                _viewModel.RemoveNeighborCount(neighborCount);
                button.SetPressedNoSignal(false);
            }

            button.IsEnabled = isAllowedNeighborCount;

            UpdateButtonSelfModulate(button);
        }
    }

    private static void UpdateButtonSelfModulate(BaseButton button) => button.ToggleSelfModulate(Colors.Green, Colors.White);
    #endregion

    public void Unsubscribe() => TriggerBinder.Unbind(this);
}